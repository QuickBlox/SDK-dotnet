using System;
using System.Collections.Generic;
using System.Text;
using QMunicate.Core.Command;

namespace QMunicate.ViewModels
{
    public class FirstViewModel : ViewModel
    {
        #region Ctor

        public FirstViewModel()
        {
            FacebookSignUpCommand = new RelayCommand(FacebookSignUpCommandExecute);
            EmailSignUpCommand = new RelayCommand(EmailSingUpCommandExecute);
            LoginCommand = new RelayCommand(LoginCommandExecute);
        }

        #endregion

        #region Properties

        public RelayCommand FacebookSignUpCommand { get; set; }
        public RelayCommand EmailSignUpCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        #endregion

        #region Private methods

        private void FacebookSignUpCommandExecute()
        {

        }

        private void EmailSingUpCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.SignUp);
        }

        private void LoginCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Login);
        }

        #endregion

    }
}
