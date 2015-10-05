using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Filters
{
    public class FieldFilter<T> : Filter
    {
        private readonly Expression<Func<T>> selectFieldExpression;
        private readonly object findValue;

        public string FormatString
        {
            get
            {
                return "{0}={1}";
            }
        }

        public FieldFilter(Expression<Func<T>> selectFieldExpression, object findValue)
        {
            this.selectFieldExpression = selectFieldExpression;
            this.findValue = findValue;
        }

        /// <summary>
        /// Builds the filter.
        /// </summary>
        /// <returns>Formated string</returns>
        internal override string BuildFilter()
        {
            var memberExpression = (MemberExpression)this.selectFieldExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            if (findValue.GetType().IsArray)
            {
                var list = new List<object>();
                foreach (var item in (Array)findValue)
                {
                    list.Add(item);
                }
                var stringValue = String.Join(",", list);             
                var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, stringValue);
            }
            else
            {                
                var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, this.findValue);
            }
        }
    }
}
