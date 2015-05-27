using System;
using System.Collections.ObjectModel;
using System.Net;
using Windows.UI.Xaml.Navigation;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;
using QMunicate;
using QMunicate.Models;

namespace QMunicate.ViewModels
{
    public class ChatsViewModel : ViewModel
    {

        public ChatsViewModel()
        {
            Dialogs = new ObservableCollection<DialogVm>();
        }

        public ObservableCollection<DialogVm> Dialogs { get; set; }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadDialogs();
        }

        private async void LoadDialogs()
        {
            RetrieveDialogsRequest retrieveDialogsRequest = new RetrieveDialogsRequest();
            var response = await QuickbloxClient.ChatClient.GetDialogsAsync(retrieveDialogsRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Dialog dialog in response.Result.Items)
                {
                    Dialogs.Add((DialogVm)dialog);
                }
            }
        }

    }
}
