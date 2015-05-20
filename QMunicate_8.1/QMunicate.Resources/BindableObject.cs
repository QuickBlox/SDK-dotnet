using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace QMunicate.Resources
{
    /// <summary>
    /// Contains logic for notifying UI about property change.  
    /// </summary>
    [DataContract]
    public class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event used for notifying bindings about property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise PropertyChanged for specified property mane
        /// </summary>
        /// <param name="propertyName">Name of changed property</param>
        protected void RaisePropertyChanged([CallerMemberName]String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
