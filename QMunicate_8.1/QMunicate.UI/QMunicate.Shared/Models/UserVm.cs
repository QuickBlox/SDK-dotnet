using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace QMunicate.Models
{
    public class UserVm
    {
        #region Ctor

        public UserVm()
        {
        }

        protected UserVm(User user)
        {
            FullName = user.FullName;
        }

        protected UserVm(Contact contact)
        {
            FullName = contact.Name;
        }

        #endregion

        #region Properties

        public string FullName { get; set; }

        public ImageSource Image { get; set; }

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
