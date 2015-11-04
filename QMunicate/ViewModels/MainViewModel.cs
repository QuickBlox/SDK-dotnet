using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DialogsAndSearchViewModel DialogsAndSearchViewModel { get; set; }
    }
}
