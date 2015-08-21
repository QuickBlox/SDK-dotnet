using QMunicate.Core.DependencyInjection;
using QMunicate.Models;
using Quickblox.Logger;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;

namespace QMunicate.Helper
{
    public class DialogsManager : IDialogsManager
    {
        #region Fields

        private bool isReloadingDialogs;
        private readonly IQuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        public DialogsManager(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
            quickbloxClient.MessagesClient.OnMessageReceived += MessagesClientOnOnMessageReceived;
            Dialogs = new ObservableCollection<DialogVm>();
        }

        #endregion

        #region Properties

        public ObservableCollection<DialogVm> Dialogs { get; private set; }

        #endregion

        #region Public methods

        public async Task ReloadDialogs()
        {
            if (isReloadingDialogs) return;
            isReloadingDialogs = true;

            try
            {
                var retrieveDialogsRequest = new RetrieveDialogsRequest();
                var response = await quickbloxClient.ChatClient.GetDialogsAsync(retrieveDialogsRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Dialogs.Clear();
                    foreach (var dialog in response.Result.Items)
                    {
                        if (dialog.Type == DialogType.Private)
                        {
                            var dialogVm = DialogVm.FromDialog(dialog);
                            int otherUserId = dialogVm.OccupantIds.FirstOrDefault(o => o != quickbloxClient.CurrentUserId);
                            dialogVm.Name = GetUserName(otherUserId);

                            Dialogs.Add(dialogVm);
                        }
                        else if(dialog.Type == DialogType.Group)
                        {
                            var dialogVm = DialogVm.FromDialog(dialog);

                            if (!string.IsNullOrEmpty(dialogVm.Photo))
                            {
                                var imagesService = ServiceLocator.Locator.Get<IImageService>();
                                dialogVm.Image = await imagesService.GetPublicImage(dialogVm.Photo);
                            }

                            Dialogs.Add(dialogVm);
                        }
                    }

                    var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
                    foreach (DialogVm dialogVm in Dialogs)
                    {
                        if (dialogVm.DialogType == DialogType.Private)
                        {
                            int otherUserId = dialogVm.OccupantIds.FirstOrDefault(o => o != quickbloxClient.CurrentUserId);
                            var user = await cachingQbClient.GetUserById(otherUserId);
                            if (user != null)
                            {
                                dialogVm.Name = user.FullName;
                                dialogVm.PrivatePhotoId = user.BlobId;
                            }
                        }
                    }
                }
            }
            finally
            {
                isReloadingDialogs = false;
            }
        }

        public async Task UpdateDialog(string dialogId, string lastActivity, DateTime lastMessageSent)
        {
            var dialog = Dialogs.FirstOrDefault(d => d.Id == dialogId);
            if (dialog != null)
            {
                dialog.LastActivity = lastActivity;
                dialog.LastMessageSent = lastMessageSent;
                int itemIndex = Dialogs.IndexOf(dialog);
                Dialogs.Move(itemIndex, 0);
            }
            else
            {
                await FileLogger.Instance.Log(LogLevel.Warn, "The dialog wasn't found in DialogsManager. Reloading dialogs.");
                await ReloadDialogs();
            }
        }

        public async Task LoadDialogImages(int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (DialogVm dialogVm in Dialogs.Where(d => d.DialogType == DialogType.Private))
            {
                if(dialogVm.PrivatePhotoId.HasValue)
                    dialogVm.Image = await imagesService.GetPrivateImage(dialogVm.PrivatePhotoId.Value, decodePixelWidth, decodePixelHeight);
            }
        }

        public void UnloadDialogImages()
        {
            foreach (DialogVm dialogVm in Dialogs.Where(d => d.DialogType == DialogType.Private))
            {
                dialogVm.Image = null;
            }
        }

        #endregion

        #region Private methods

        private void MessagesClientOnOnMessageReceived(object sender, Message message)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await UpdateDialog(message.DialogId, message.MessageText, message.DateTimeSent);
            });
        }

        private string GetUserName(int userId)
        {
            var otherContact = quickbloxClient.MessagesClient.Contacts.FirstOrDefault(c => c.UserId == userId);
            if (otherContact != null)
                return otherContact.Name;

            return null;
        }

        #endregion

    }
}
