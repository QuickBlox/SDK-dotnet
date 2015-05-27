namespace QMunicate.ViewModels
{
    public class ViewModelLocator
    {
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

        public ChatsViewModel ChatsViewModel
        {
            get { return new ChatsViewModel(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}