using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;

namespace QMunicate.ViewModels
{
    public class SignUpViewModel : ViewModel
    {
        private string fullName;
        private string email;
        private string password;
        private ImageSource userImage;

        public SignUpViewModel()
        {
            ChoosePhotoCommand = new RelayCommand(ChoosePhotoCommandExecute);
            SignUpCommand = new RelayCommand(SignUpCommandExecute);
        }

        public string FullName
        {
            get { return fullName; }
            set { Set(ref fullName, value); }
        }

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        public ImageSource UserImage
        {
            get { return userImage; }
            set { Set(ref userImage, value); }
        }

        public ICommand ChoosePhotoCommand { get; set; }

        public ICommand SignUpCommand { get; set; }


        public override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        private async void ChoosePhotoCommandExecute()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.PickSingleFileAndContinue();
            
        }

        private void SignUpCommandExecute()
        {

        }
    }
}
