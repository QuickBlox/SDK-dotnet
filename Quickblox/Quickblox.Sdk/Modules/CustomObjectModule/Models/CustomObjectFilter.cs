using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Models
{
    public class CustomObjectFilter<T> : Filter
    {
        private readonly CustomObjectOperator customObjectOperator;
        private readonly Expression<Func<T>> selectFieldExpression;
        private readonly object findValue;
        
        public string FormatString
        {
            get
            {
                return "{0}[{1}]={2}";
            }
        }

        public CustomObjectFilter(CustomObjectOperator customObjectOperator, Expression<Func<T>> selectFieldExpression, String findValue)
        {
            this.customObjectOperator = customObjectOperator;
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

            var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();

            return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, this.customObjectOperator.ToString().ToLower(), this.findValue);
        }
    }
}
