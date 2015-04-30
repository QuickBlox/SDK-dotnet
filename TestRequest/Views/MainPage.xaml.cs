using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Quickblox.Sdk.Core;

namespace TestRequest.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //var uriBase = "https://api.quickblox.com";
            //var appId = "21183";
            //var qbKey = "suLpnYcW2UtK6N7BcRq2";
            //var authKey = "LxnQksQJsXA2NLU";
            //var authSecret = "7v2Jkrc7e-99JJX";
            //var quickbloxclient = new QuickbloxClient(uriBase, appId, qbKey, authKey, authSecret);
            //quickbloxclient.ClientStatusChanged += this.QuickbloxclientOnClientStatusChanged;
            //await quickbloxclient.InitializeClientAsync();
        }

        private void QuickbloxclientOnClientStatusChanged(object sender, bool b)
        {
        }
    }
}
