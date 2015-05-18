namespace QMunicate.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            
        }

        public SignUpViewModel SignUpViewModel
        {
            get { return new SignUpViewModel(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return new LoginViewModel(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}