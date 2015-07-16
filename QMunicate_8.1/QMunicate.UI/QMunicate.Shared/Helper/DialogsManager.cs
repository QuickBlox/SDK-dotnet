using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Core.DependencyInjection;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Helper
{
    public class DialogsManager : IDialogsManager
    {
        private readonly IQuickbloxClient quickbloxClient;

        public ObservableCollection<DialogVm> Dialogs { get; private set; }

        public DialogsManager(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient; //TODO: subscribe to xmpp and update dialogs on xmpp events
            Dialogs = new ObservableCollection<DialogVm>();
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
