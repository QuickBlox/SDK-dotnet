using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Helper;
using QMunicate.Models;

namespace QMunicate.ViewModels
{
    public class GroupInfoViewModel : ViewModel
    {
        #region Fields

        private string chatName;
        private ImageSource chatImage;

        #endregion

        #region Ctor

        public GroupInfoViewModel()
        {
            Participants = new ObservableCollection<UserVm>();
            AddMembersCommand = new RelayCommand(AddMembersCommandExecute, () => !IsLoading);
            EditCommand = new RelayCommand(EditCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public string ChatName
        {
            get { return chatName; }
            set { Set(ref chatName, value); }
        }

        public ImageSource ChatImage
        {
            get { return chatImage; }
            set { Set(ref chatImage, value); }
        }

        public ObservableCollection<UserVm> Participants { get; set; }

        public RelayCommand AddMembersCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dialog = e.Parameter as DialogVm;
            if (dialog == null) return;

            await Initialize(dialog);
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            AddMembersCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async Task Initialize(DialogVm dialog)
        {
            IsLoading = true;
            ChatName = dialog.Name;
            ChatImage = dialog.Image;

            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var imagesService = ServiceLocator.Locator.Get<IImageService>();

            foreach (int occupantId in dialog.OccupantIds)
            {
                var user = await cachingQbClient.GetUserById(occupantId);
                if (user != null)
                    Participants.Add(UserVm.FromUser(user));
            }

            foreach (UserVm userVm in Participants)
            {
                if (userVm.ImageUploadId.HasValue)
                {
                    userVm.Image = await imagesService.GetPrivateImage(userVm.ImageUploadId.Value);
                }
            }

            IsLoading = false;
        }

        private void AddMembersCommandExecute()
        {

        }

        private void EditCommandExecute()
        {

        }

        #endregion

    }
}
