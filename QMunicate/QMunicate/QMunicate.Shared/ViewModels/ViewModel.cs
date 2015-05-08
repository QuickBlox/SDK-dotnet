using System;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.Navigation;
using QMunicate.Core.Observable;
using Quickblox.Sdk;

namespace QMunicate.ViewModels
{
    public class ViewModel : ObservableObject, INavigationAware
    {
        private Boolean isLoading;

        public ViewModel()
        {
            this.NavigationService = Factory.CommonFactory.GetInstance<INavigationService>();
            this.QuickbloxClient = Factory.CommonFactory.GetInstance<QuickbloxClient>();

            this.GoBackCommand = new RelayCommand(this.GoBackCommandExecute, this.CanGobackCommandExecute);
        }
        
        public Boolean IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(ref this.isLoading, value); }
        }

        public RelayCommand GoBackCommand { get; set; }

        public INavigationService NavigationService { get; protected set; }

        public QuickbloxClient QuickbloxClient { get; protected set; }


        private void GoBackCommandExecute()
        {
            this.NavigationService.GoBack();
        }
        
        private bool CanGobackCommandExecute()
        {
            return this.NavigationService.CanGoBack;
        }

        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedFrom(NavigatingCancelEventArgs e)
        {
        }
    }
}
