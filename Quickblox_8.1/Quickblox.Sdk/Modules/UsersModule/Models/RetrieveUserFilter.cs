using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Filter;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.UsersModule.Models
{
    public class RetrieveUserFilter<T> : Filter
    {
        private readonly UserOperator userOperator;
        private readonly Expression<Func<T>> selectFieldExpression;
        private readonly object findValue;

        public string ParameterName
        {
            get
            {
                return "filter[]";
            }
        }

        public string FormatString
        {
            get
            {
                return "{0}={1}+{2}+{3}+{4}";
            }
        }

        public RetrieveUserFilter(UserOperator userOperator, Expression<Func<T>> selectFieldExpression, String findValue)
        {
            this.userOperator = userOperator;
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
            var filedTypeString = GetFilterFieldTypeString(propertyInfo);

            return String.Format(this.FormatString, this.ParameterName, filedTypeString, jsonPropertyAttribute.PropertyName, this.userOperator.ToString().ToLower(), this.findValue);
        }
    }
}
