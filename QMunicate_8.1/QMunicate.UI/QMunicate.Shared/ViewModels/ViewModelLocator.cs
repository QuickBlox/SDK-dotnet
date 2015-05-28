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

        public DialogsViewModel ChatsViewModel
        {
            get { return new DialogsViewModel(); }
        }

        public ChatViewModel ChatViewModel
        {
            get { return new ChatViewModel(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}