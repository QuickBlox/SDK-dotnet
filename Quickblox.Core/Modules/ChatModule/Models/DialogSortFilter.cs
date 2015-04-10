using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.ChatModule.Models
{
    public class DialogSortFilter<T> : Filter
    {
        private readonly SortOperator sortOperator;
        private readonly Expression<Func<T>> selectFieldExpression;

        public string ParameterName
        {
            get
            {
                return "sort_";
            }
        }

        public string FormatString
        {
            get
            {
                return "{0}{1}={2}";
            }
        }

        public DialogSortFilter(SortOperator sortOperator, Expression<Func<T>> selectFieldExpression)
        {
            this.sortOperator = sortOperator;
            this.selectFieldExpression = selectFieldExpression;
        }

        internal override string BuildFilter()
        {
            var memberExpression = (MemberExpression)this.selectFieldExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;

            var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();

            return String.Format(this.FormatString, this.ParameterName, this.sortOperator.ToString().ToLower(), jsonPropertyAttribute.PropertyName);
        }
    }
}
