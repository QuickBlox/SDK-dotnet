using QMunicate.Core.Command;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;

namespace QMunicate.ViewModels
{
    public class DialogsViewModel : ViewModel
    {
        private int currentUserId;

        #region Ctor

        public DialogsViewModel()
        {
            Dialogs = new ObservableCollection<DialogVm>();
            OpenChatCommand = new RelayCommand<object>(OpenChatCommandExecute);
            SignOutCommand = new RelayCommand(SignOutCommandExecute);
            NewMessageCommand = new RelayCommand(NewMessageCommandExecute);
            SearchCommand = new RelayCommand(SearchCommandExecute);
        }

        #endregion

        #region Properties

        public ObservableCollection<DialogVm> Dialogs { get; set; }

        public RelayCommand<object> OpenChatCommand { get; set; }

        public ICommand NewMessageCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as DialogsNavigationParameter;
            if (parameter != null && e.NavigationMode != NavigationMode.Back)
            {
                NavigationService.BackStack.Clear();
                currentUserId = parameter.CurrentUserId;
                
                await Initialize(parameter.CurrentUserId, parameter.Password);
            }
        }

        #endregion

        #region Private methods

        private async Task Initialize(int userId, string password)
        {
            IsLoading = true;
            await ConnectToChat(userId, password);
            await LoadDialogs();
            IsLoading = false;
        }

        private async Task ConnectToChat(int userId, string password)
        {
            if (!QuickbloxClient.MessagesClient.IsConnected)
                await QuickbloxClient.MessagesClient.Connect(QuickbloxClient.ChatEndpoint, userId, ApplicationKeys.ApplicationId, password);
        }

        private async Task LoadDialogs()
        {
            Dialogs.Clear();
            RetrieveDialogsRequest retrieveDialogsRequest = new RetrieveDialogsRequest();
            var response = await QuickbloxClient.ChatClient.GetDialogsAsync(retrieveDialogsRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Dialog dialog in response.Result.Items)
                {
                    Dialogs.Add((DialogVm) dialog);
                }
            }
        }

        private async void SignOutCommandExecute()
        {
            try
            {
                var passwordVault = new PasswordVault();
                var credentials = passwordVault.FindAllByResource(ApplicationKeys.QMunicateCredentials);
                if (credentials != null && credentials.Any())
                {
                    passwordVault.Remove(credentials[0]);
                }
                }
            catch (Exception)
            {
            }

            NavigationService.Navigate(ViewLocator.SignUp);
            NavigationService.BackStack.Clear();
        }

        private void OpenChatCommandExecute(object dialog)
        {
            NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter {CurrentUserId = currentUserId, Dialog = dialog as DialogVm});
        }

        private void NewMessageCommandExecute()
        {

        }

        private void SearchCommandExecute()
        {

        }

        #endregion

    }
}
