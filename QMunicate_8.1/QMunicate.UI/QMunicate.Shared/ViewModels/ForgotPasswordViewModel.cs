using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Input;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using Quickblox.Sdk;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace QMunicate.ViewModels
{
    public class ForgotPasswordViewModel : ViewModel
    {
        #region Fields

        private string email;

        #endregion

        #region Ctor

        public ForgotPasswordViewModel()
        {
            ResetCommand = new RelayCommand(ResetCommandExecute);
        }

        #endregion

        #region Properties

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }

        public ICommand ResetCommand { get; private set; }

        #endregion

        #region Private methods

        private async void ResetCommandExecute()
        {
            await QuickbloxClient.CoreClient.CreateSessionBaseAsync(ApplicationKeys.ApplicationId,
                        ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret,
                        new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() });
            var response = await QuickbloxClient.UsersClient.ResetUserPasswordByEmailAsync(Email);

            var messageService = Factory.CommonFactory.GetInstance<IMessageService>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await messageService.ShowAsync("Reset", "A link to reset your password was sent to your email.");
            }

            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                await messageService.ShowAsync("Not found", "The user with this email wasn't found.");
            }
        }

        #endregion

    }
}
