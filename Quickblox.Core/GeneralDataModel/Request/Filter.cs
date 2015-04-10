using System;
using System.Reflection;

namespace Quickblox.Sdk.GeneralDataModel.Request
{
    public abstract class Filter
    {
        protected static string GetFilterFieldTypeString(PropertyInfo propertyInfo)
        {
            String filedTypeString = null;
            if (propertyInfo.PropertyType == typeof(String))
            {
                filedTypeString = "string";
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                filedTypeString = "date";
            }
            else
            {
                filedTypeString = "number";
            }

            return filedTypeString;
        }

        public abstract string BuildFilter();
    }
}
