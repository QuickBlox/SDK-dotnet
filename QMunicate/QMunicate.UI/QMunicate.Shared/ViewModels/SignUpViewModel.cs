using QMunicate.Core.Command;
using Quickblox.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using QMunicate.Helper;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace QMunicate.ViewModels
{
    public class SignUpViewModel : ViewModel, IFileOpenPickerContinuable
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
#if WINDOWS_PHONE_APP
            picker.PickSingleFileAndContinue();
#endif
        }

        private async void SignUpCommandExecute()
        {
            await QuickbloxClient.CoreClient.CreateSessionBaseAsync(ApplicationKeys.ApplicationId,
                        ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret,
                        new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() });

            var response = await QuickbloxClient.UsersClient.SignUpUserAsync(FullName, Password, email: Email);
        }

        public async void ContinueFileOpenPicker(IReadOnlyList<StorageFile> files)
        {
            await QuickbloxClient.CoreClient.CreateSessionBaseAsync(ApplicationKeys.ApplicationId,
                        ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret,
                        new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() });

            if (files != null && files.Any())
            {
                FileRandomAccessStream stream = (FileRandomAccessStream)await files[0].OpenAsync(FileAccessMode.Read);

                var bytes = new byte[stream.Size];

                using (var dataReader = new DataReader(stream))
                {
                    await dataReader.LoadAsync((uint)stream.Size);
                    dataReader.ReadBytes(bytes);
                }

                await QuickbloxClient.ContentClient.UploadFile(bytes, "image/jpg");


                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                UserImage = bitmapImage;
            }
        }

    }
}
