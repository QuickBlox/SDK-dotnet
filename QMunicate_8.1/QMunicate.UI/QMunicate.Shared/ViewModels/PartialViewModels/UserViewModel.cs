using Windows.UI.Xaml.Media;
using QMunicate.Core.Observable;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace QMunicate.ViewModels.PartialViewModels
{
    public class UserViewModel : ObservableObject
    {
        private ImageSource image;

        #region Ctor

        public UserViewModel()
        {
        }

        protected UserViewModel(User user)
        {
            UserId = user.Id;
            FullName = user.FullName;
            ImageUploadId = user.BlobId;
        }

        protected UserViewModel(Contact contact)
        {
            UserId = contact.UserId;
            FullName = contact.Name;
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        public string FullName { get; set; }

        public int? ImageUploadId { get; set; }

        public ImageSource Image
        {
            get { return image; }
            set { Set(ref image, value); }
        }

        #endregion

        #region Public methods

        public static UserViewModel FromUser(User user)
        {
            return new UserViewModel(user);
        }

        public static UserViewModel FromContact(Contact contact)
        {
            return new UserViewModel(contact);
        }

        #endregion

    }
}
