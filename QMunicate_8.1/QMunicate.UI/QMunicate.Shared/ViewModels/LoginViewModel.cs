using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Input;
using Windows.Security.Credentials;
using QMunicate.Core.Command;
using QMunicate.Helper;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace QMunicate.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        #region Fields

        private string email;
        private string password;
        private bool rememberMe;

        #endregion

        #region Ctor

        public LoginViewModel()
        {
            ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
            LoginCommand = new RelayCommand(LoginCommandExecute);

            Email = "an@to.ly";
            Password = "12345678";
        }

        #endregion

        #region Properties

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        public bool RememberMe
        {
            get { return rememberMe; }
            set { Set(ref rememberMe, value); }
        }

        public ICommand ForgotPasswordCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        #endregion

        #region Private methods

        private void ForgotPasswordCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.ForgotPassword);
        }

        private async void LoginCommandExecute()
        {
            if (RememberMe)
            {
                var passwordVault = new PasswordVault();
                var credentials = new PasswordCredential(ApplicationKeys.QMunicateCredentials, Email, Password);
                passwordVault.Add(credentials);
            }

            var response = await QuickbloxClient.CoreClient.CreateSessionWithEmailAsync(ApplicationKeys.ApplicationId,
                ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret, Email, Password,
                deviceRequestRequest:new DeviceRequest() {Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId()});

            if (response.StatusCode == HttpStatusCode.Created)
            {
                //QuickbloxClient.MessagesClient.Connect(response.Result.User.Id, Password, ApplicationKeys.ApplicationId,
                //    QuickbloxClient.ChatEndpoint);
                this.NavigationService.Navigate(ViewLocator.Chats, response.Result.Session.UserId);
            }
        }

        #endregion

    }
}
