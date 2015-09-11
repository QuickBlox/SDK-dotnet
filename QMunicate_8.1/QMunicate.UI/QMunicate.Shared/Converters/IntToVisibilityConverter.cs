﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace QMunicate.Converters
{
    public sealed class IntToVisibilityConverter : IValueConverter
    {
        #region Properties

        public Boolean Invert { get; set; }

        #endregion

        #region IValueConverter Members

        /// <summary>
        /// Convert Boolean or Nullable to Visibility
        /// </summary>
        /// <param name="value">bool or Nullable</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">null</param>
        /// <param name="language">null</param>
        /// <returns>Visible or Collapsed</returns>
        public object Convert(Object value, Type targetType, Object parameter, String language)
        {
            bool isVisible = false;

            if (value is int)
            {
                int intValue = (int) value;
                isVisible = intValue != 0;

                if (Invert)
                    isVisible = !isVisible;
            }
            
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert Visibility to Boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
