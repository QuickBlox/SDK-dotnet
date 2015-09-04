﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Facebook.Client;
using QMunicate.Core.Command;
using QMunicate.Models;
using Quickblox.Sdk;

namespace QMunicate.ViewModels
{
    public class FirstViewModel : ViewModel
    {
        #region Ctor

        public FirstViewModel()
        {
            FacebookSignUpCommand = new RelayCommand(FacebookSignUpCommandExecute, () => !IsLoading);
            EmailSignUpCommand = new RelayCommand(EmailSingUpCommandExecute, () => !IsLoading);
            LoginCommand = new RelayCommand(LoginCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public RelayCommand FacebookSignUpCommand { get; set; }
        public RelayCommand EmailSignUpCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            FacebookSignUpCommand.RaiseCanExecuteChanged();
            EmailSignUpCommand.RaiseCanExecuteChanged();
            LoginCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private void FacebookSignUpCommandExecute()
        {
            Session.OnFacebookAuthenticationFinished += OnFacebookAuthenticationFinished;
            Session.ActiveSession.LoginWithBehavior("public_profile", FacebookLoginBehavior.LoginBehaviorMobileInternetExplorerOnly);
        }

        private async void OnFacebookAuthenticationFinished(AccessTokenData fbSession)
        {
            IsLoading = true;
            var sessionResponse = await QuickbloxClient.CoreClient.CreateSessionWithSocialNetworkKey(ApplicationKeys.ApplicationId, ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret, "facebook",
                                                                "public_profile", fbSession.AccessToken, null, null);
            if (sessionResponse.StatusCode == HttpStatusCode.Created)
            {
                QuickbloxClient.Token = sessionResponse.Result.Session.Token;
                SettingsManager.Instance.WriteToSettings(SettingsKeys.CurrentUserId, sessionResponse.Result.Session.UserId);
                NavigationService.Navigate(ViewLocator.Dialogs,
                                                    new DialogsNavigationParameter
                                                    {
                                                        CurrentUserId = sessionResponse.Result.Session.UserId,
                                                        Password = sessionResponse.Result.Session.Token
                                                    });
            }

            IsLoading = false;
        }

        private void EmailSingUpCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.SignUp);
        }

        private void LoginCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Login);
        }

        #endregion

    }
}
