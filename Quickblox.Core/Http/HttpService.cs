using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Http;
using Quickblox.Sdk.GeneralDataModel;

namespace Quickblox.Sdk.Http
{
    public class HttpService : HttpBase
    {
        #region Public Members

        #region Get

        public static async Task<HttpResponse<TResult>> GetAsync<TResult>(String baseAddress, String requestUri,
            ISerializer serializer, IDictionary<String, IEnumerable<String>> headers = null,
            CancellationToken token = default(CancellationToken))
        {
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> GetAsync<TResult, TSettings>(String baseAddress, String requestUri, ISerializer serializer, TSettings requestSettings, 
            IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken)) where TSettings : BaseRequestSettings
        {
            if (requestSettings != null)
            {
                if (requestUri == null) requestUri = "";
                requestUri += "?" + UrlBuilder.Build(requestSettings);
            }
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> GetAsync<TResult>(String baseAddress, String requestUri, ISerializer serializer, IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken))
        {
            if (nameValueCollection != null)
            {
                if (requestUri == null) requestUri = "";
                requestUri += "?" + UrlBuilder.Build(nameValueCollection);
            }
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #region Post

        public static async Task<HttpResponse<TResult>> PostAsync<TResult>(String baseAddress, String requestUri,
            ISerializer serializer, IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IDictionary<String, IEnumerable<String>> headers = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response =
                    await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult, TSettings>(String baseAddress,
            String requestUri, ISerializer serializer, TSettings settings,
            IDictionary<String, IEnumerable<String>> headers = null,
            CancellationToken token = default(CancellationToken)) where TSettings : BaseRequestSettings
        {
            HttpResponseMessage response;
            var body = serializer.Serialize(settings);
            using (var httpContent = new StringContent(body, Encoding.UTF8, serializer.ContentType))
            {
                response = await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token)
                            .ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult>(String baseAddress, String requestUri,
            ISerializer serializer, Byte[] data, IDictionary<String, IEnumerable<String>> headers = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var multiPartContent = new MultipartFormDataContent())
            {
                var imageContent = new ByteArrayContent(data);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                multiPartContent.Add(imageContent, "image", "image.jpg");
                response =
                    await PostBaseAsync(baseAddress, requestUri, multiPartContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #region Delete

        public static async Task<HttpResponse<TResult>> DeleteAsync<TResult>(String baseAddress, String requestUri,
            ISerializer serializer, IDictionary<String, IEnumerable<String>> headers = null,
            CancellationToken token = default(CancellationToken))
        {
            var response = await DeleteBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #region Put

        public static async Task<HttpResponse<TResult>> PutAsync<TResult>(String baseAddress, String requestUri,
            ISerializer serializer, IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response =
                    await PutBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PutAsync<TResult, TSettings>(String baseAddress, String requestUri,
            ISerializer serializer, TSettings settings,
            IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            var body = serializer.Serialize(settings);
            using (var httpContent = new StringContent(body, Encoding.UTF8, serializer.ContentType))
            {
                response = await PutBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #endregion

        private static async Task<HttpResponse<TResult>> ParseResult<TResult>(ISerializer serializer, HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Debug.WriteLine(String.Concat("CONTENT: ", stringContent));
            HttpResponse<TResult> httpResponse = new HttpResponse<TResult>();

            if (response.IsSuccessStatusCode)
            {
                httpResponse.Result = serializer.Deserialize<TResult>(stringContent);
            }
            else
            {
                try
                {
                    var error = serializer.Deserialize<ErrorResponse>(stringContent);
                    httpResponse.Error = error.Error;
                }
                catch (Exception)
                {
                    httpResponse.Error = new Error() {Text = new[] {stringContent}};
                }
        }

            httpResponse.StatusCode = response.StatusCode;
            return httpResponse;
        }
    }
}
