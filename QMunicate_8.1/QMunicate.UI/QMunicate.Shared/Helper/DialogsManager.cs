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
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Helper
{
    public class DialogsManager : IDialogsManager
    {
        private readonly IQuickbloxClient quickbloxClient;

        public ObservableCollection<DialogVm> Dialogs { get; private set; }

        public DialogsManager(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
            quickbloxClient.MessagesClient.OnMessageReceived += MessagesClientOnOnMessageReceived;
            Dialogs = new ObservableCollection<DialogVm>();
        }

        private void MessagesClientOnOnMessageReceived(object sender, Message message)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var dialog = Dialogs.FirstOrDefault(d => d.Id == message.DialogId);
                if (dialog != null)
                {
                    dialog.LastActivity = message.MessageText;
                    dialog.LastMessageSent = message.DateTimeSent;
                }
                else
                {
                    await ReloadDialogs();
                }
            });
        }

        public async Task ReloadDialogs()
        {
            var retrieveDialogsRequest = new RetrieveDialogsRequest();
            var response = await quickbloxClient.ChatClient.GetDialogsAsync(retrieveDialogsRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Dialogs.Clear();
                foreach (var dialog in response.Result.Items)
                {
                    Dialogs.Add(DialogVm.FromDialog(dialog));
                }
            }
        }
    }
}
