using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Core.DependencyInjection;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Helper
{
    public class DialogsManager : IDialogsManager
    {
        public List<Dialog> Dialogs { get; private set; }

        public DialogsManager()
        {
            Dialogs = new List<Dialog>();
        }

        public async Task ReloadDialogs()
        {
            var quickbloxClient = ServiceLocator.Locator.Get<IQuickbloxClient>();
            var retrieveDialogsRequest = new RetrieveDialogsRequest();
            var response = await quickbloxClient.ChatClient.GetDialogsAsync(retrieveDialogsRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Dialogs.Clear();
                Dialogs = response.Result.Items.ToList();
            }
        }
    }
}
