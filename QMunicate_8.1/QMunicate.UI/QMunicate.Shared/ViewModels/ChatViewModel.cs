using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Models;

namespace QMunicate.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        #region Fields

        private string newMessageText;
        private string chatName;

        #endregion

        #region Ctor

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageVm>();
            SendCommand = new RelayCommand(SendCommandExecute);
        }

        #endregion

        #region Properties

        public ObservableCollection<MessageVm> Messages { get; set; }

        public string NewMessageText
        {
            get { return newMessageText; }
            set { Set(ref newMessageText, value); }
        }

        public string ChatName
        {
            get { return chatName; }
            set { Set(ref chatName, value); }
        }

        public ICommand SendCommand { get; set; }

        #endregion

        #region Navigation

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dialog = e.Parameter as DialogVm;
            if (dialog != null)
                ChatName = dialog.Name;
        }

        #endregion

        #region Private methods

        private async void SendCommandExecute()
        {
            if (string.IsNullOrEmpty(NewMessageText)) return;

            Messages.Add(new MessageVm()
            {
                MessageText = NewMessageText,
                MessageType = MessageType.Outgoing,
                DateTime = DateTime.Now
            });

            NewMessageText = "";
        }

        #endregion

    }
}
