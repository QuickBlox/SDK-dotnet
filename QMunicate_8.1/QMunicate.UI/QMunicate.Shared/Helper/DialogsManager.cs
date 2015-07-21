using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using QMunicate.Core.DependencyInjection;
using QMunicate.Models;
using Quickblox.Logger;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;
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
                        //TODO: remove this when group chats are ready
                        if (dialog.Type != DialogType.Private)
                        {
                            await FileLogger.Instance.Log(LogLevel.Debug, "Ignoring not private chat");
                            continue;
                        }

                        var dialogVm = DialogVm.FromDialog(dialog);
                        int otherUserId = dialogVm.OccupantIds.FirstOrDefault(o => o != quickbloxClient.CurrentUserId);
                        dialogVm.Name = await GetUserName(otherUserId);

                        Dialogs.Add(dialogVm);
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

        #endregion

        #region Private methods

        private void MessagesClientOnOnMessageReceived(object sender, Message message)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await UpdateDialog(message.DialogId, message.MessageText, message.DateTimeSent);
            });
        }

        private async Task<string> GetUserName(int userId)
        {
            var otherContact = quickbloxClient.MessagesClient.Contacts.FirstOrDefault(c => c.UserId == userId);
            if (otherContact != null)
                return otherContact.Name;

            var response = await quickbloxClient.UsersClient.GetUserByIdAsync(userId);
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Result.User.FullName;

            return null;
        }

        #endregion

    }
}
