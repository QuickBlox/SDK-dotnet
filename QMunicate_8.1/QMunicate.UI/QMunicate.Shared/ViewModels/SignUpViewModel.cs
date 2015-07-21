using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.GeneralDataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace QMunicate.ViewModels
{
    public class SignUpViewModel : ViewModel, IFileOpenPickerContinuable
    {
        #region Fields

        private string fullName;
        private string email;
        private string password;
        private ImageSource userImage;
        private readonly IMessageService messageService;

        #endregion

        #region Ctor

        public SignUpViewModel()
        {
            messageService = ServiceLocator.Locator.Get<IMessageService>();

            ChoosePhotoCommand = new RelayCommand(ChoosePhotoCommandExecute, () => !IsLoading);
            SignUpCommand = new RelayCommand(SignUpCommandExecute, () => !IsLoading);
            LoginCommand = new RelayCommand(LoginCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

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

        public RelayCommand ChoosePhotoCommand { get; set; }

        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand LoginCommand { get; set; }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            ChoosePhotoCommand.RaiseCanExecuteChanged();
            SignUpCommand.RaiseCanExecuteChanged();
            LoginCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Public methods

        public async void ContinueFileOpenPicker(IReadOnlyList<StorageFile> files)
        {
            await CreateSession();

            if (files != null && files.Any())
            {
                FileRandomAccessStream stream = (FileRandomAccessStream) await files[0].OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                UserImage = bitmapImage;

                //CONTENT: {"errors":{"base":["Forbidden. Need user."]}}

                //var bytes = new byte[stream.Size];
                //using (var dataReader = new DataReader(stream))
                //{
                //    await dataReader.LoadAsync((uint)stream.Size);
                //    dataReader.ReadBytes(bytes);
                //}

                //await QuickbloxClient.ContentClient.UploadFile(bytes, "image/jpg");
            }
        }

        #endregion

        #region Private methods

        private async Task CreateSession()
        {
            if (!string.IsNullOrEmpty(QuickbloxClient.Token)) return;

            var sessionResponse = await QuickbloxClient.CoreClient.CreateSessionBaseAsync(ApplicationKeys.ApplicationId,
                ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret, new DeviceRequest() {Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId()});
            if (sessionResponse.StatusCode == HttpStatusCode.Created)
            {
                QuickbloxClient.Token = sessionResponse.Result.Session.Token;
            }
            else
            {
                await Helpers.ShowErrors(sessionResponse.Errors, messageService);
            }
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
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await messageService.ShowAsync("Message", "Please fill all empty input fields");
                return;
            }

            IsLoading = true;

            await CreateSession();

            var response = await QuickbloxClient.UsersClient.SignUpUserAsync(null, Password, email: Email, fullName: FullName);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var loginResponse = await QuickbloxClient.CoreClient.ByEmailAsync(Email, Password);
                if (loginResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    QuickbloxClient.CurrentUserId = loginResponse.Result.User.Id;

                    NavigationService.Navigate(ViewLocator.Dialogs,
                        new DialogsNavigationParameter
                        {
                            CurrentUserId = loginResponse.Result.User.Id,
                            Password = Password
                        });
                }
                else await Helpers.ShowErrors(response.Errors, messageService);
            }
            else await Helpers.ShowErrors(response.Errors, messageService);

            IsLoading = false;
        }

        private void LoginCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Login);
        }

        #endregion

    }
}
