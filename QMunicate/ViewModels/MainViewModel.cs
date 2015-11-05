using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace QMunicate.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Ctor

        public MainViewModel()
        {
            DialogsAndSearchViewModel = new DialogsAndSearchViewModel();
        }

        #endregion

        #region Properties

        public DialogsAndSearchViewModel DialogsAndSearchViewModel { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsLoading = true;
            await DialogsAndSearchViewModel.Initialize(e.Parameter);
            IsLoading = false;
        }

        #endregion
    }
}
