using System;
using System.Collections.Generic;
using System.Text;

namespace QMunicate.ViewModels
{
    public class SearchViewModel : ViewModel
    {
        private string searchText;

        public string SearchText
        {
            get { return searchText; }
            set { Set(ref searchText, value); }
        }
    }
}
