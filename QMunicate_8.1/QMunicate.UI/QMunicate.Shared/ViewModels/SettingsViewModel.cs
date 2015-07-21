using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using Quickblox.Logger;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.Models;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace QMunicate.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        #region Fields

        private bool isPushEnabled;
        private bool isSettingPushEnabledFromCode;

        #endregion

        #region Ctor

        public SettingsViewModel()
        {
            SignOutCommand = new RelayCommand(SignOutCommandExecute, () => !IsLoading);
            DeleteAccountCommand = new RelayCommand(DeleteAccountCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public bool IsPushEnabled
        {
            get { return isPushEnabled; }
            set
            {
                if (!isSettingPushEnabledFromCode)
                {
                    ChangePushsEnabled(value);
                }
                else
                {
                    isSettingPushEnabledFromCode = false;
                }
                Set(ref isPushEnabled, value);
                
            }
        }

        public RelayCommand SignOutCommand { get; set; }

        public RelayCommand DeleteAccountCommand { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            int pushSubscriptionId = SettingsManager.Instance.ReadFromSettings<int>(SettingsKeys.PushSubscriptionId);
            if (pushSubscriptionId != default(int))
            {
                isSettingPushEnabledFromCode = true;
                IsPushEnabled = true;
            }
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SignOutCommand.RaiseCanExecuteChanged();
            DeleteAccountCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async void ChangePushsEnabled(bool newValue)
        {
            IsLoading = true;

            var pushNotificationsManager = ServiceLocator.Locator.Get<IPushNotificationsManager>();

            if (newValue)
            {
                SettingsManager.Instance.WriteToSettings(SettingsKeys.UserDisabledPush, false);

                bool isEnabled = await pushNotificationsManager.CreateSubscriptionIfNeeded();
                if (!isEnabled)
                {
                    isSettingPushEnabledFromCode = true;
                    IsPushEnabled = false;
                }
            }
            else
            {
                SettingsManager.Instance.WriteToSettings(SettingsKeys.UserDisabledPush, true);
                await pushNotificationsManager.DeleteSubscription();
            }
            IsLoading = false;
        }

        private async void SignOutCommandExecute()
        {
            IsLoading = true;
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            DialogCommand logoutCommand = new DialogCommand("logout", new RelayCommand(SignOut));
            DialogCommand cancelCommand = new DialogCommand("cancel", new RelayCommand(() => { }), false, true);
            await messageService.ShowAsync("Logout", "Do you really want to logout?", new [] {logoutCommand, cancelCommand});
            IsLoading = false;
        }

        private async void SignOut()
        {
            IsLoading = true;

            await TurnOffPushNotifications();

            QuickbloxClient.MessagesClient.Disconnect();
            await QuickbloxClient.CoreClient.DeleteCurrentSession();

            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            dialogsManager.Dialogs.Clear();

            DeleteStoredCredentials();
            
            IsLoading = false;
            NavigationService.Navigate(ViewLocator.SignUp);
            NavigationService.BackStack.Clear();
        }

        private async Task TurnOffPushNotifications()
        {
            var pushNotificationsManager = ServiceLocator.Locator.Get<IPushNotificationsManager>();
            await pushNotificationsManager.DeleteSubscription();
            await pushNotificationsManager.DeletePushToken();

            SettingsManager.Instance.DeleteFromSettings(SettingsKeys.UserDisabledPush);
        }

        private void DeleteStoredCredentials()
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
            catch (Exception) { }
        }

        private async void DeleteAccountCommandExecute()
        {

        }

        #endregion
    }
}
