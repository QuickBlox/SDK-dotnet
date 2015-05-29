namespace QMunicate.ViewModels
{
    public class ViewModelLocator
    {
        #region Fields

        private DialogsViewModel dialogsViewModel;

        #endregion

        #region Properties

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

        public DialogsViewModel DialogsViewModel
        {
            get { return dialogsViewModel ?? (dialogsViewModel = new DialogsViewModel()); }
        }

        public ChatViewModel ChatViewModel
        {
            get { return new ChatViewModel(); }
        }

        #endregion

        #region Public methods

        public void Cleanup()
        {
            dialogsViewModel = null;
        }

        #endregion

    }
}