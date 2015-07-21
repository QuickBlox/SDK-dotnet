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
            DialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            OpenChatCommand = new RelayCommand<object>(OpenChatCommandExecute, obj => !IsLoading);
            NewMessageCommand = new RelayCommand(NewMessageCommandExecute, () => !IsLoading);
            SearchCommand = new RelayCommand(SearchCommandExecute, () => !IsLoading);
            SettingsCommand = new RelayCommand(SettingsCommandExecute, () => !IsLoading);
            InviteFriendsCommand = new RelayCommand(InviteFriendsCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public IDialogsManager DialogsManager { get; set; }

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
            if (!QuickbloxClient.MessagesClient.IsConnected)
            {
                await QuickbloxClient.MessagesClient.Connect(QuickbloxClient.ChatEndpoint, userId, ApplicationKeys.ApplicationId, password);
                QuickbloxClient.MessagesClient.ReloadContacts();
            }
        }

        private async Task LoadDialogs()
        {
            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            if(!dialogsManager.Dialogs.Any()) await dialogsManager.ReloadDialogs();
        }

        #region Push notifications

        private async Task InitializePush()
        {
            var pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            pushChannel.PushNotificationReceived += PushChannelOnPushNotificationReceived;

            var pushNotificationsManager = ServiceLocator.Locator.Get<IPushNotificationsManager>();
            await pushNotificationsManager.UpdatePushTokenIfNeeded(pushChannel);

            bool userDisabledPush = SettingsManager.Instance.ReadFromSettings<bool>(SettingsKeys.UserDisabledPush);
            if (!userDisabledPush)
                await pushNotificationsManager.CreateSubscriptionIfNeeded();
        }

        private async void PushChannelOnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var messageService = ServiceLocator.Locator.Get<IMessageService>();
                await messageService.ShowAsync("Message", "Push received");
            });
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
