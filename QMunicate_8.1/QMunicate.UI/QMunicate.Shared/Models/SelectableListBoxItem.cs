using System;
using System.Collections.Generic;
using System.Text;
using QMunicate.Core.Observable;

namespace QMunicate.Models
{
    public class SelectableListBoxItem<T> : ObservableObject
    {
        private bool isSelected;
        private T item;

        public SelectableListBoxItem(T item)
        {
            if(item == null) throw new ArgumentNullException("item");

            Item = item;
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(ref isSelected, value); }
        }

        public T Item
        {
            get { return item; }
            set { Set(ref item, value); }
        }
    }
}
