using System;
using System.Linq;
using System.Net;
using System.Text;

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
        public static String Build(BaseRequestSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            var builder = new StringBuilder();
            var dictionary = settings.Parameters.ToDictionary(key => key.Key, value => value.Value);
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

        #endregion
    }
}
