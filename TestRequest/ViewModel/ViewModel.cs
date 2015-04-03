using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Quickblox.Sdk;

namespace TestRequest.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private Boolean isLoading;

        public Boolean IsLoading
        {
            get { return this.isLoading; }
            set { Set(ref this.isLoading, value); }
        }

        public INavigationService NavigationService { get; protected set; }

        public QuickbloxClient QuickbloxClient { get; protected set; }
    }
}
