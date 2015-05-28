using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using QMunicate.Models;

namespace QMunicate.Selectors
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OutgoingMessageTemplate { get; set; }

        public DataTemplate IncomingMessageTemplate { get; set; }


        /// <summary>
        /// When overridden in a derived class, returns a DataTemplate based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a DataTemplate object or null.</returns>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var message = item as MessageVm;
            if (message != null)
            {
                return message.MessageType == MessageType.Incoming ? IncomingMessageTemplate : OutgoingMessageTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
