using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.ChatModule.Models
{
    public class RetriveDialogsFilter<T> : Filter
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

        public RetriveDialogsFilter(Expression<Func<T>> selectFieldExpression, object findValue)
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

            var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
            return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, this.findValue);
        }
    }
}
