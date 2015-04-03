using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Quickblox.Sdk.Http
{
    /// <summary>
    /// HttpHeadersExtensions class.
    /// </summary>
    static class HttpHeadersExtensions
    {
        /// <summary>
        /// Длбавляет новый заголовок и его значения в коллекцию HttpHeaders без проверки предоставленных сведений.
        /// </summary>
        /// <param name="httpRequestHeaders">Представляет коллекцию заголовков запроса, как определено в RFC 2616.</param>
        /// <param name="headers">Коллекция заголовков и их значения, как указано в RFC 2616.</param>
        public static void SetRequestHeaders(this HttpHeaders httpRequestHeaders, IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers)
        {
            if (httpRequestHeaders == null) throw new ArgumentNullException("httpRequestHeaders");
            if (headers == null) return;
            foreach (var header in headers)
            {
                if (httpRequestHeaders.TryAddWithoutValidation(header.Key, header.Value))
                    continue;
                throw new HttpRequestException(String.Format("Failed to add a header {0}", header.Key));
            }
        }
    }
}