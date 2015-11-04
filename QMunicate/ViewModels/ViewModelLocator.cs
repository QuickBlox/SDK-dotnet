namespace QMunicate.ViewModels
{
    public class ViewModelLocator
    {
        #region Properties

        public WelcomeViewModel WelcomeViewModel => new WelcomeViewModel();

        public SignUpViewModel SignUpViewModel => new SignUpViewModel();

        public LoginViewModel LoginViewModel => new LoginViewModel();

        public ForgotPasswordViewModel ForgotPasswordViewModel => new ForgotPasswordViewModel();

        public MainViewModel MainViewModel => new MainViewModel();

        #endregion
    }
}