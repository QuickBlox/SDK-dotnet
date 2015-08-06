using Nito.AsyncEx;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.MessageService;

namespace QMunicate.ViewModels
{
    public class GroupAddMemberViewModel : ViewModel
    {
        private string groupName;
        private string searchText;
        private string membersText;
        private readonly AsyncLock contactsLock = new AsyncLock();
        private List<SelectableListBoxItem<UserVm>> allContacts;

        #region Ctor

        public GroupAddMemberViewModel()
        {
            Contacts = new ObservableCollection<SelectableListBoxItem<UserVm>>();
            CreateGroupCommand = new RelayCommand(CreateGroupCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public string GroupName
        {
            get { return groupName; }
            set { Set(ref groupName, value); }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (Set(ref searchText, value))
                    Search(searchText);
            }
        }

        public string MembersText
        {
            get { return membersText; }
            set { Set(ref membersText, value); }
        }

        public ObservableCollection<SelectableListBoxItem<UserVm>> Contacts { get; set; }

        public RelayCommand CreateGroupCommand { get; set; }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            IsLoading = true;
            await InitializeAllContacts();
            await Search(null);
            IsLoading = false;
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            CreateGroupCommand.RaiseCanExecuteChanged();
        }

        #endregion

        private async Task InitializeAllContacts()
        {
            allContacts = new List<SelectableListBoxItem<UserVm>>();
            foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
            {
                var userVm = UserVm.FromContact(contact);
                allContacts.Add(new SelectableListBoxItem<UserVm>(userVm));
            }

            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (var userVm in allContacts)
            {
                var userResponse = await QuickbloxClient.UsersClient.GetUserByIdAsync(userVm.Item.UserId);
                if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result.User.BlobId.HasValue)
                {
                    userVm.Item.Image = await imagesService.GetPrivateImage(userResponse.Result.User.BlobId.Value);
                }
            }
        }

        private async Task Search(string searchQuery)
        {
            using (await contactsLock.LockAsync())
            {
                Contacts.Clear();
                if (string.IsNullOrEmpty(searchQuery))
                {
                    foreach (var userVm in allContacts)
                    {
                        Contacts.Add(userVm);
                    }
                }
                else
                {
                    foreach (var userVm in allContacts.Where(c => !string.IsNullOrEmpty(c.Item.FullName) && c.Item.FullName.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        Contacts.Add(userVm);
                    }
                }
            }

            
        }

        private async void CreateGroupCommandExecute()
        {
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                await messageService.ShowAsync("Group name", "A Group name field must not be empty.");
                return;
            }

            IsLoading = true;
            var userIdsBuilder = new StringBuilder();
            foreach (var contact in allContacts.Where(c => c.IsSelected))
            {
                userIdsBuilder.Append(contact.Item.UserId + ",");
            }
            if (userIdsBuilder.Length == 0)
            {
                await messageService.ShowAsync("No users", "Please, select some users to add to the group.");
                IsLoading = false;
                return;
            }
            userIdsBuilder.Remove(userIdsBuilder.Length - 1, 1);

            var createDialogResponse = await QuickbloxClient.ChatClient.CreateDialogAsync(GroupName, DialogType.Group, userIdsBuilder.ToString());
            if (createDialogResponse.StatusCode == HttpStatusCode.Created)
            {
                ChatNavigationParameter chatNavigationParameter = new ChatNavigationParameter();
                chatNavigationParameter.Dialog = DialogVm.FromDialog(createDialogResponse.Result);
                var groupChatManager = QuickbloxClient.MessagesClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid);
                groupChatManager.JoinGroup(QuickbloxClient.CurrentUserId.ToString());
                var isGroupMessageSent = groupChatManager.SendMessage("A new group chat was created");
                if(isGroupMessageSent)
                    NavigationService.Navigate(ViewLocator.GroupChat, chatNavigationParameter);
            }

            IsLoading = false;
        }
    }
}
