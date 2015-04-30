using System;
using System.Net;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Quickblox.Sdk;

namespace TestRequest.ViewModel
{
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService, QuickbloxClient quickbloxClient)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            this.NavigationService = navigationService;
            this.QuickbloxClient = quickbloxClient;
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
                var response = await this.QuickbloxClient.CoreClient.ByLoginAsync(this.Login, this.Password);
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    QuickbloxClient.MessagesClient.Connect(response.Result.User.Id, Password, ApplicationKeys.ApplicationId, QuickbloxClient.ChatEndpoint);
                    this.NavigationService.NavigateTo("Chats", response.Result.User.Id);
                }
            }
            finally 
            {
                this.IsLoading = false;
            }
        }
    }
}