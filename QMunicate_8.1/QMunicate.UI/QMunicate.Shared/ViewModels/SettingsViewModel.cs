using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using Quickblox.Sdk;
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
            SignOutCommand = new RelayCommand(SignOutCommandExecute);
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

        public ICommand SignOutCommand { get; set; }

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

        #region Private methods

        private async void ChangePushsEnabled(bool newValue)
        {
            IsLoading = true;
            if (newValue)
            {
                SettingsManager.Instance.WriteToSettings(SettingsKeys.UserDisabledPush, false);

                var createSubscriptionsResponse = await QuickbloxClient.NotificationClient.CreateSubscriptionsAsync(NotificationChannelType.mpns);
                if (createSubscriptionsResponse.StatusCode == HttpStatusCode.Created)
                {
                    var subscription = createSubscriptionsResponse.Result.FirstOrDefault();
                    if (subscription != null)
                        SettingsManager.Instance.WriteToSettings(SettingsKeys.PushSubscriptionId, subscription.Subscription.Id);
                }
                else
                {
                    isSettingPushEnabledFromCode = true;
                    IsPushEnabled = false;
                }
            }
            else
            {
                SettingsManager.Instance.WriteToSettings(SettingsKeys.UserDisabledPush, true);

                int pushSubscriptionId = SettingsManager.Instance.ReadFromSettings<int>(SettingsKeys.PushSubscriptionId);
                if (pushSubscriptionId != default(int))
                {
                    var deleteResponse = await QuickbloxClient.NotificationClient.DeleteSubscriptionsAsync(pushSubscriptionId);
                    if(deleteResponse.StatusCode == HttpStatusCode.OK)
                        SettingsManager.Instance.DeleteFromSettings(SettingsKeys.PushSubscriptionId);
                }
            }
            IsLoading = false;
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

        #endregion
    }
}
