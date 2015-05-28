using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;
using QMunicate;
using QMunicate.Core.Command;
using QMunicate.Models;

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
        }

        #endregion

        #region Properties

        public ObservableCollection<DialogVm> Dialogs { get; set; }

        public RelayCommand<object> OpenChatCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        #endregion

        #region Navigation

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int)
                currentUserId = (int) e.Parameter;

            NavigationService.BackStack.Clear();
            LoadDialogs();
        }

        #endregion

        #region Private methods

        private async void LoadDialogs()
        {
            Dialogs = new ObservableCollection<DialogVm>();
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

        #endregion

    }
}
