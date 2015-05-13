using System.Net;
using QMunicate.Core.Command;
using QMunicate.Helper;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace QMunicate.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            this.LoginCommand = new RelayCommand(this.LoginExecute, () => !this.IsLoading && !string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.Password));

            this.Login = "Test654321";
            this.Password = "Test12345";
        }

        private string login;

        public string Login
        {
            get { return this.login; }
            set
            {
                this.Set(ref this.login, value);
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }
        
        private string password;

        public string Password
        {
            get { return this.password; }
            set
            {
                this.Set(ref this.password, value);
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LoginCommand { get; private set; }
        
        private async void LoginExecute()
        {
            this.IsLoading = true;
            try
            {
                await
                    QuickbloxClient.CoreClient.CreateSessionBaseAsync(ApplicationKeys.ApplicationId,
                        ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret,
                        new DeviceRequest() {Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId()});

                var response = await this.QuickbloxClient.CoreClient.ByLoginAsync(this.Login, this.Password);
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    QuickbloxClient.MessagesClient.Connect(response.Result.User.Id, Password, ApplicationKeys.ApplicationId, QuickbloxClient.ChatEndpoint);
                    this.NavigationService.Navigate("ChatsPage", response.Result.User.Id);
                }
            }
            finally 
            {
                this.IsLoading = false;
            }
        }
    }
}