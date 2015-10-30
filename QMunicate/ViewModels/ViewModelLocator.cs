namespace QMunicate.ViewModels
{
    public class ViewModelLocator
    {
        #region Properties

        public WelcomeViewModel WelcomeViewModel
        {
            get { return new WelcomeViewModel(); }
        }

        public SignUpViewModel SignUpViewModel
        {
            get { return new SignUpViewModel(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return new LoginViewModel(); }
        }

        public ForgotPasswordViewModel ForgotPasswordViewModel
        {
            get { return new ForgotPasswordViewModel(); }
        }

        #endregion
    }
}