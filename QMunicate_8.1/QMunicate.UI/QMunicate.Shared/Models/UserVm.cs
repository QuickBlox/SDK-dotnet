using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;
using QMunicate.Core.Observable;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Models;
using XMPP.tags.jabber.protocol.resultset;

namespace QMunicate.Models
{
    public class UserVm : ObservableObject
    {
        private ImageSource image;

        #region Ctor

        public UserVm()
        {
        }

        protected UserVm(User user)
        {
            UserId = user.Id;
            FullName = user.FullName;
            ImageUploadId = user.BlobId;
        }

        protected UserVm(Contact contact)
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

        public static UserVm FromUser(User user)
        {
            return new UserVm(user);
        }

        public static UserVm FromContact(Contact contact)
        {
            return new UserVm(contact);
        }

        #endregion

    }
}
