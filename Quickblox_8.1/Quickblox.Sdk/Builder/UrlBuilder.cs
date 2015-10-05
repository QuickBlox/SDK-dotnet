using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Builder
{
    /// <summary>
    /// UrlBuilder class.
    /// </summary>
    internal static class UrlBuilder
    {
        #region Public Members

        //<summary>
        //Формирует строку запроса.
        //</summary>
        //<param name="settings">Настройки запроса.</param>
        //<returns>Строка запроса в виде Url.</returns>
        public static String Build(IEnumerable<KeyValuePair<String, String>> nameValueCollection)
        {
            if (nameValueCollection == null) throw new ArgumentNullException("nameValueCollection");
            var builder = new StringBuilder();
            var dictionary = nameValueCollection.ToDictionary(key => key.Key, value => value.Value);
            var flag = false;
            foreach (var property in dictionary.Where(item => !String.IsNullOrEmpty(item.Value)))
            {
                if (flag)
                {
                    builder.Append(String.Format("&{0}={1}", property.Key, WebUtility.UrlEncode(property.Value)));
                    continue;
                }
                builder.Append(String.Format("{0}={1}", property.Key, WebUtility.UrlEncode(property.Value)));
                flag = true;
            }
            
            return String.Format(builder.ToString());
        }

        public static String Build(BaseRequestSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            var properties = settings.GetType().GetRuntimeProperties();
            var navBody = new StringBuilder();
            var flag = false;
            foreach (
                var property in properties.Where(pr => pr.GetCustomAttribute<JsonPropertyAttribute>() != null).OrderBy(pr => pr.GetCustomAttribute<JsonPropertyAttribute>().PropertyName))
            {
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();

                if (property.PropertyType.GetTypeInfo().Namespace.Contains("System"))
                {
                    var value = property.GetValue(settings);

                    if (value == null) continue;

                    if (flag)
                    {
                        navBody.Append(String.Format("&{0}={1}", attribute.PropertyName, value));
                        continue;
                    }
                    navBody.Append(String.Format("&{0}={1}", attribute.PropertyName, value));
                    flag = true;
                }
            }

            if (settings.Filter != null)
            {
                navBody.Append(settings.Filter.BuildFilter());
            }

            return navBody.ToString();
        }

        public static String BuildFilter(FilterAggregator aggregators)
        {
            var navBody = new StringBuilder();
            foreach (var aggregator in aggregators.Filters)
            {
                navBody.Append(aggregator.BuildFilter());
            }
           

            return navBody.ToString();
        }

        #endregion
    }
}
