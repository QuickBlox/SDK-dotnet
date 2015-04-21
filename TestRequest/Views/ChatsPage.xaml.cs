
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TestRequest.ViewModel;

namespace TestRequest.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChatsPage : Page
    {
        public ChatsPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as INavigatable;
            if(viewModel != null)
                viewModel.OnNavigated(e.Parameter);
        }
    }
}
