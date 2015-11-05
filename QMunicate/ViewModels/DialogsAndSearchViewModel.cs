using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Core.Command;

namespace QMunicate.ViewModels
{
    public class DialogsAndSearchViewModel : ViewModel
    {
        #region Fields

        private string searchText;
        private bool isSearchMode;

        #endregion

        #region Ctor

        public DialogsAndSearchViewModel()
        {
            NewGroupCommand = new RelayCommand(NewGroupCommandExecute, () => !IsLoading);
            SettingsCommand = new RelayCommand(SettingsCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (Set(ref searchText, value))
                    OnSearchTextChanged();
            }
        }

        public RelayCommand NewGroupCommand { get; set; }

        public RelayCommand SettingsCommand { get; set; }

        public bool IsSearchMode
        {
            get { return isSearchMode; }
            set { Set(ref isSearchMode, value); }
        }

        #endregion

        #region Private methods

        private void OnSearchTextChanged()
        {
            if (!string.IsNullOrEmpty(SearchText) && !IsSearchMode)
                IsSearchMode = true;

            if (string.IsNullOrEmpty(SearchText) && IsSearchMode)
                IsSearchMode = false;
        }

        private void NewGroupCommandExecute()
        {

        }

        private void SettingsCommandExecute()
        {

        } 
        #endregion
    }
}
