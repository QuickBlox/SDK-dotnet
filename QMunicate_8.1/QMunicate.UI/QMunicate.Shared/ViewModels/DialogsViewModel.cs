﻿using QMunicate.Core.Command;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.Models;
using Quickblox.Sdk.Modules.NotificationModule.Models;
using Quickblox.Sdk.Modules.NotificationModule.Requests;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.PushNotifications;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using Environment = Quickblox.Sdk.Modules.NotificationModule.Models.Environment;

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
                
                await InitializeChat(parameter.CurrentUserId, parameter.Password);
            }
            await InitializePush();
        }

        #endregion

        #region Private methods

        private async Task InitializeChat(int userId, string password)
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

        #region Push notifications

        private async Task InitializePush()
        {
            var pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            pushChannel.PushNotificationReceived += PushChannelOnPushNotificationReceived;
            await CheckAndUpdatePushToken(pushChannel);
            await CreatePushSubscriptionIfNeeded();
        }

        private void PushChannelOnPushNotificationReceived(PushNotificationChannel sender,
            PushNotificationReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async Task CheckAndUpdatePushToken(PushNotificationChannel pushChannel)
        {
            string tokenHash = Helpers.ComputeMD5(pushChannel.Uri);
            string storedTokenHash = SettingsManager.Instance.ReadFromSettings<string>(SettingsKeys.PushTokenHash);
            if (tokenHash != storedTokenHash)
            {
                string storedTokenId = SettingsManager.Instance.ReadFromSettings<string>(SettingsKeys.PushTokenId);
                if (storedTokenId != null)
                {
                    var deleteResponse = await QuickbloxClient.NotificationClient.DeletePushTokenAsync(storedTokenId);
                    if (deleteResponse.StatusCode != HttpStatusCode.OK) return;
                }

                var settings = new CreatePushTokenRequest()
                {
                    DeviceRequest =
                        new DeviceRequest() {Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId()},
                    PushToken =
                        new PushToken()
                        {
                            Environment = Environment.production,
                            ClientIdentificationSequence = pushChannel.Uri
                        }
                };
                var createPushTokenResponse = await QuickbloxClient.NotificationClient.CreatePushTokenAsync(settings);
                if (createPushTokenResponse.StatusCode == HttpStatusCode.Created)
                {
                    SettingsManager.Instance.WriteToSettings(SettingsKeys.PushTokenId,
                        createPushTokenResponse.Result.PushToken.PushTokenId);
                    SettingsManager.Instance.WriteToSettings(SettingsKeys.PushTokenHash, tokenHash);
                }
            }
        }

        private async Task CreatePushSubscriptionIfNeeded()
        {
            bool subscriptionCreated =
                SettingsManager.Instance.ReadFromSettings<bool>(SettingsKeys.IsPushSubscriptionCreated);
            if (!subscriptionCreated)
            {
                var createSubscriptionsResponse =
                    await QuickbloxClient.NotificationClient.CreateSubscriptionsAsync(NotificationChannelType.mpns);
                if (createSubscriptionsResponse.StatusCode == HttpStatusCode.Created)
                {
                    SettingsManager.Instance.WriteToSettings(SettingsKeys.IsPushSubscriptionCreated, true);
                }
            }
        }

        #endregion

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

        private async void NewMessageCommandExecute()
        {
            var pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            pushChannel.PushNotificationReceived += PushChannelOnPushNotificationReceived;




        }

        private void SearchCommandExecute()
        {

        }

        #endregion

    }
}
