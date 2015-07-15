using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
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
using Windows.ApplicationModel.Core;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Environment = Quickblox.Sdk.Modules.NotificationModule.Models.Environment;

namespace QMunicate.ViewModels
{
    public class DialogsViewModel : ViewModel
    {
        #region Ctor

        public DialogsViewModel()
        {
            Dialogs = new ObservableCollection<DialogVm>();
            OpenChatCommand = new RelayCommand<object>(OpenChatCommandExecute, obj => !IsLoading);
            NewMessageCommand = new RelayCommand(NewMessageCommandExecute, () => !IsLoading);
            SearchCommand = new RelayCommand(SearchCommandExecute, () => !IsLoading);
            SettingsCommand = new RelayCommand(SettingsCommandExecute, () => !IsLoading);
            InviteFriendsCommand = new RelayCommand(InviteFriendsCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public ObservableCollection<DialogVm> Dialogs { get; set; }

        public RelayCommand<object> OpenChatCommand { get; set; }

        public RelayCommand NewMessageCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand InviteFriendsCommand { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsLoading = true;
            var parameter = e.Parameter as DialogsNavigationParameter;
            if (parameter != null && e.NavigationMode != NavigationMode.Back)
            {
                NavigationService.BackStack.Clear();
                await InitializeChat(parameter.CurrentUserId, parameter.Password);
            }
            await LoadDialogs();
            await InitializePush();
            IsLoading = false;
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            OpenChatCommand.RaiseCanExecuteChanged();
            NewMessageCommand.RaiseCanExecuteChanged();
            SearchCommand.RaiseCanExecuteChanged();
            SettingsCommand.RaiseCanExecuteChanged();
            InviteFriendsCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async Task InitializeChat(int userId, string password)
        {
            await ConnectToChat(userId, password);
            QuickbloxClient.MessagesClient.ReloadContacts();
        }

        private async Task ConnectToChat(int userId, string password)
        {
            if (!QuickbloxClient.MessagesClient.IsConnected)
                await QuickbloxClient.MessagesClient.Connect(QuickbloxClient.ChatEndpoint, userId, ApplicationKeys.ApplicationId, password);
        }

        private async Task LoadDialogs()
        {
            Dialogs.Clear();

            QuickbloxClient.MessagesClient.OnContactRequestReceived += (sender, request) =>
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Dialogs.Add(DialogVm.FromContactRequest(request, QuickbloxClient.CurrentUserId));
                });
            };
            foreach (ContactRequest contactRequest in QuickbloxClient.MessagesClient.ContactRequests)
            {
                Dialogs.Add(DialogVm.FromContactRequest(contactRequest, QuickbloxClient.CurrentUserId));
            }
            

            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            if(!dialogsManager.Dialogs.Any()) await dialogsManager.ReloadDialogs();
            dialogsManager.Dialogs = dialogsManager.Dialogs.OrderByDescending(d => d.LastMessageDateSent).ToList();
            foreach (Dialog dialog in dialogsManager.Dialogs)
            {
                //int otherUserId = dialog.OccupantsIds.FirstOrDefault(id => id != QuickbloxClient.CurrentUserId);
                Dialogs.Add(DialogVm.FromDialog(dialog));
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

        private async void PushChannelOnPushNotificationReceived(PushNotificationChannel sender,
            PushNotificationReceivedEventArgs args)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var messageService = ServiceLocator.Locator.Get<IMessageService>();
                await messageService.ShowAsync("Message", "Push received");
            });
        }

        private async Task CheckAndUpdatePushToken(PushNotificationChannel pushChannel)
        {
            string tokenHash = Helpers.ComputeMD5(pushChannel.Uri);
            string storedTokenHash = SettingsManager.Instance.ReadFromSettings<string>(SettingsKeys.PushTokenHash);
            if (tokenHash != storedTokenHash)
            {
                string storedTokenId = SettingsManager.Instance.ReadFromSettings<string>(SettingsKeys.PushTokenId);
                if (!string.IsNullOrEmpty(storedTokenId))
                {
                    var deleteResponse = await QuickbloxClient.NotificationClient.DeletePushTokenAsync(storedTokenId);
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
            int pushSubscriptionId = SettingsManager.Instance.ReadFromSettings<int>(SettingsKeys.PushSubscriptionId);
            bool userDisabledPush = SettingsManager.Instance.ReadFromSettings<bool>(SettingsKeys.UserDisabledPush);
            if (pushSubscriptionId == default(int) && !userDisabledPush)
            {
                var createSubscriptionsResponse = await QuickbloxClient.NotificationClient.CreateSubscriptionsAsync(NotificationChannelType.mpns);
                if (createSubscriptionsResponse.StatusCode == HttpStatusCode.Created)
                {
                    var subscription = createSubscriptionsResponse.Result.FirstOrDefault();
                    if (subscription != null)
                    {
                        SettingsManager.Instance.WriteToSettings(SettingsKeys.PushSubscriptionId, subscription.Subscription.Id);
                    }
                    else
                    {
                        var subscriptions = await QuickbloxClient.NotificationClient.GetSubscriptionsAsync();
                        if (subscriptions.StatusCode == HttpStatusCode.OK)
                        {
                            var subs = subscriptions.Result.FirstOrDefault(s => s.Subscription != null && s.Subscription.NotificationChannel != null && s.Subscription.NotificationChannel.Name == NotificationChannelType.mpns);
                            if (subs != null)
                                SettingsManager.Instance.WriteToSettings(SettingsKeys.PushSubscriptionId, subs.Subscription.Id);
                        }
                    }

                    
                }
            }
        }

        #endregion

        private void OpenChatCommandExecute(object dialog)
        {
            NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter {Dialog = dialog as DialogVm});
        }

        private async void NewMessageCommandExecute()
        {

        }

        private void SearchCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Search);
        }

        private void SettingsCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Settings);
        }

        private void InviteFriendsCommandExecute()
        {

        }

        #endregion

    }
}
