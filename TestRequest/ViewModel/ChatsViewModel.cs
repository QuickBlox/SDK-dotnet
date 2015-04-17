using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Quickblox.Sdk;

namespace TestRequest.ViewModel
{
    public class ChatsViewModel : ViewModel, INavigatable
    {
        private INavigationService navigationService;
        private QuickbloxClient quickbloxClient;

        public ChatsViewModel(INavigationService navigationService, QuickbloxClient quickbloxClient)
        {
            this.navigationService = navigationService;
            this.quickbloxClient = quickbloxClient;
        }

        public void OnNavigated()
        {
            var a = quickbloxClient.MessagesClient.InitPrivateChatManager(2766517);
            a.OnInitialized += AOnOnInitialized;
        }

        private void AOnOnInitialized(object sender, EventArgs eventArgs)
    {

        }
    }
}
