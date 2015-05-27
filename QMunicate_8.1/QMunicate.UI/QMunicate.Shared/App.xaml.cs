using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageBoxProvider;
using QMunicate.Core.Navigation;
using QMunicate.Helper;
using QMunicate.Views;
using Quickblox.Sdk;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Hmacsha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace QMunicate
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Factory.CommonFactory.Bind<INavigationService, NavigationService>(LifetimeMode.Singleton);
            Factory.CommonFactory.Bind<QuickbloxClient, QuickbloxClient>(LifetimeMode.Singleton);
            Factory.CommonFactory.Bind<IMessageBoxProvider, MessageBoxProvider>(LifetimeMode.Singleton);

            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// 
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            var navigationService = Factory.CommonFactory.GetInstance<INavigationService>();
            navigationService.Initialize(rootFrame, this.GetPageResolver());

            var quickbloxClient = Factory.CommonFactory.GetInstance<QuickbloxClient>();
            await quickbloxClient.InitializeClientAsync(ApplicationKeys.ApiBaseEndPoint, ApplicationKeys.AccountKey, new HmacSha1CryptographicProvider());

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
                HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
#endif

                await DoFirstNavigation(quickbloxClient, navigationService, e);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            var quickbloxClient = Factory.CommonFactory.GetInstance<QuickbloxClient>();
            var token = SettingsManager.Instance.ReadFromSettings<string>(SettingsKeys.QbToken);
            quickbloxClient.Resume(token);

#if WINDOWS_PHONE_APP

            var continuationActivatedEventArgs = args as IContinuationActivatedEventArgs;
            if (args != null)
            {
                ContinuationManager.Continue(continuationActivatedEventArgs);
            }
#endif
        }


        private async Task DoFirstNavigation(QuickbloxClient quickbloxClient, INavigationService navigationService, LaunchActivatedEventArgs e)
        {
            string login = null;
            string password = null;

            try
            {
                var passwordVault = new PasswordVault();
                var credentials = passwordVault.FindAllByResource(ApplicationKeys.QMunicateCredentials);
                if (credentials != null && credentials.Any())
                {
                    credentials[0].RetrievePassword();
                    login = credentials[0].UserName;
                    password = credentials[0].Password;
                }
            }
            catch (Exception)
            {
            }

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                var response = await quickbloxClient.CoreClient.CreateSessionWithEmailAsync(ApplicationKeys.ApplicationId,
                        ApplicationKeys.AuthorizationKey, ApplicationKeys.AuthorizationSecret, login, password,
                        deviceRequestRequest: new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() });
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    navigationService.Navigate(ViewLocator.SignUp, e.Arguments); //TODO: navigate to proper page
                }
            }
            else
            {
                navigationService.Navigate(ViewLocator.SignUp, e.Arguments);
            }
        }


        private PageResolver GetPageResolver()
        {
            var dictionary = new Dictionary<string, Type>();
            dictionary.Add("Main", typeof(MainPage));
            dictionary.Add(ViewLocator.SignUp, typeof(SignUpPage));
            dictionary.Add(ViewLocator.Login, typeof(LoginPage));
            dictionary.Add(ViewLocator.ForgotPassword, typeof(ForgotPasswordPage));
            //dictionary.Add("ChatsPage", typeof(ChatsPage));
            return new PageResolver(dictionary);
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            var navigationService = Factory.CommonFactory.GetInstance<INavigationService>();
            if (navigationService != null && navigationService.CanGoBack)
            {
                navigationService.GoBack();
                backPressedEventArgs.Handled = true;
            }
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();

            var quickbloxClient = Factory.CommonFactory.GetInstance<QuickbloxClient>();

            SettingsManager.Instance.WriteToSettings(SettingsKeys.QbToken, quickbloxClient.Token);
        }
    }
}