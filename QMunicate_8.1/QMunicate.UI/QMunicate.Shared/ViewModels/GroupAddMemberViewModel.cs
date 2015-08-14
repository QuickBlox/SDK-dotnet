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
using Quickblox.Sdk.Modules.ChatModule.Requests;

namespace QMunicate.ViewModels
{
    public class GroupAddMemberViewModel : ViewModel
    {
        private string groupName;
        private string searchText;
        private string membersText;
        private readonly AsyncLock contactsLock = new AsyncLock();
        private List<SelectableListBoxItem<UserVm>> allContacts;
        private bool isEditMode;
        private DialogVm editedDialog;

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

        public bool IsEditMode
        {
            get { return isEditMode; }
            set { Set(ref isEditMode, value); }
        }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var dialog = e.Parameter as DialogVm;
            if (dialog != null)
            {
                IsEditMode = true;
                editedDialog = dialog;
            }

            IsLoading = true;
            await InitializeAllContacts(editedDialog);
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

        private async Task InitializeAllContacts(DialogVm existingDialog)
        {
            allContacts = new List<SelectableListBoxItem<UserVm>>();
            foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
            {
                var userVm = UserVm.FromContact(contact);
                allContacts.Add(new SelectableListBoxItem<UserVm>(userVm));
            }
            if (existingDialog != null)
            {
                var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
                foreach (int occupantId in existingDialog.OccupantIds)
                {
                    var correspondingContact = allContacts.FirstOrDefault(c => c.Item.UserId == occupantId);
                    if (correspondingContact != null)
                    {
                        correspondingContact.IsSelected = true;
                    }
                    else if(occupantId != QuickbloxClient.CurrentUserId)
                    {
                        var notInContactsUser = await cachingQbClient.GetUserById(occupantId);
                        if (notInContactsUser != null)
                        {
                            var selectableUser = new SelectableListBoxItem<UserVm>(UserVm.FromUser(notInContactsUser)) {IsSelected = true};
                            allContacts.Add(selectableUser);
                        }
                    }
                }
            }


            await LoadAllContactsImages();
        }

        private async Task LoadAllContactsImages()
        {
            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (var userVm in allContacts)
            {
                var user = await cachingQbClient.GetUserById(userVm.Item.UserId);
                if (user != null && user.BlobId.HasValue)
                {
                    userVm.Item.Image = await imagesService.GetPrivateImage(user.BlobId.Value);
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
            IsLoading = true;

            if (!await Validate())
            {
                IsLoading = false;
                return;
            }

            if (IsEditMode)
            {
                await UpdateGroup();
            }
            else
            {
                await CreateGroup();
            }

            IsLoading = false;
        }

        private async Task UpdateGroup()
        {
            var selectedContacts = allContacts.Where(c => c.IsSelected).ToList();

            var updateDialogRequest = new UpdateDialogRequest {DialogId = editedDialog.Id};
            var addedUsers = selectedContacts.Where(c => !editedDialog.OccupantIds.Contains(c.Item.UserId)).Select(u => u.Item.UserId).ToArray();
            var removedUsers = editedDialog.OccupantIds.Where(c => selectedContacts.All(sc => sc.Item.UserId != c) && c != QuickbloxClient.CurrentUserId).ToArray();
            if (addedUsers.Any())
                updateDialogRequest.PushAll = new EditedOccupants() { OccupantsIds = addedUsers };
            if (removedUsers.Any())
                updateDialogRequest.PullAll = new EditedOccupants() { OccupantsIds = removedUsers };

            var updateDialogResponse = await QuickbloxClient.ChatClient.UpdateDialogAsync(updateDialogRequest);

            if (updateDialogResponse.StatusCode == HttpStatusCode.OK)
            {
                ChatNavigationParameter chatNavigationParameter = new ChatNavigationParameter { Dialog = DialogVm.FromDialog(updateDialogResponse.Result) };
                NavigationService.Navigate(ViewLocator.GroupChat, chatNavigationParameter);
            }
        }

        private async Task CreateGroup()
        {
            var selectedContacts = allContacts.Where(c => c.IsSelected).ToList();
            string selectedUsersString = BuildUsersString(selectedContacts.Select(c => c.Item.UserId));

            var createDialogResponse = await QuickbloxClient.ChatClient.CreateDialogAsync(GroupName, DialogType.Group, selectedUsersString);
            if (createDialogResponse.StatusCode == HttpStatusCode.Created)
            {
                ChatNavigationParameter chatNavigationParameter = new ChatNavigationParameter { Dialog = DialogVm.FromDialog(createDialogResponse.Result) };

                foreach (var contact in selectedContacts)
                {
                    var privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(contact.Item.UserId);
                    await privateChatManager.SendNotificationMessage(createDialogResponse.Result.Id);
                }

                var groupChatManager = QuickbloxClient.MessagesClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid, createDialogResponse.Result.Id);
                groupChatManager.JoinGroup(QuickbloxClient.CurrentUserId.ToString());
                var isGroupMessageSent = groupChatManager.SendMessage("A new group chat was created");
                if (isGroupMessageSent)
                    NavigationService.Navigate(ViewLocator.GroupChat, chatNavigationParameter);
            }
        }

        private async Task<bool> Validate()
        {
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            if (!isEditMode && string.IsNullOrWhiteSpace(GroupName))
            {
                await messageService.ShowAsync("Group name", "A Group name field must not be empty.");
                return false;
            }

            if (!allContacts.Any(c => c.IsSelected))
            {
                await messageService.ShowAsync("No users", "Please, select some users to add to the group.");
                IsLoading = false;
                return false;
            }

            return true;
        }

        private string BuildUsersString(IEnumerable<int> users)
        {
            var userIdsBuilder = new StringBuilder();
            foreach (int userId in users)
            {
                userIdsBuilder.Append(userId + ",");
            }
            if(userIdsBuilder.Length > 1)
                userIdsBuilder.Remove(userIdsBuilder.Length - 1, 1);

            return userIdsBuilder.ToString();
        }
    }
}
