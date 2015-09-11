using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using System.Collections.Generic;

namespace Quickblox.Sdk.Modules.ChatModule.Models
{
    public class RetrieveDialogsFilterWithOperator<T> : Filter
    {
        private readonly DialogSearchOperator dialogSearchOperator;
        private readonly Expression<Func<T>> selectFieldExpression;
        private readonly object findValue;

        public string FormatString
        {
            get
            {
                return "{0}[{1}]={2}";
            }
        }

        public RetrieveDialogsFilterWithOperator(DialogSearchOperator dialogSearchOperator, Expression<Func<T>> selectFieldExpression, object findValue)
        {
            this.dialogSearchOperator = dialogSearchOperator;
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
                return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, this.dialogSearchOperator.ToString().ToLower(), stringValue);
            }
            else
            {
                var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                return String.Format(this.FormatString, jsonPropertyAttribute.PropertyName, this.dialogSearchOperator.ToString().ToLower(), this.findValue);
            }
         
        }
    }
}
