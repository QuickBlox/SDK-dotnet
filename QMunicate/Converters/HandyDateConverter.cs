using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace QMunicate.Converters
{
    public class HandyDateConverter : IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, String language)
        {
            var dateTimeValue = value as DateTime?;
            if (!dateTimeValue.HasValue) return null;

            return dateTimeValue.Value.Date == DateTime.Today ? "Today" : dateTimeValue.Value.Date.ToString("dd MMM yyyy");
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
