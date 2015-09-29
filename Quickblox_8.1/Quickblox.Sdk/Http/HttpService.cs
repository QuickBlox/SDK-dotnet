using Quickblox.Sdk.Builder;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Serializer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Http
{
    internal class HttpService : HttpBase
    {
        static HttpService()
        {
            FactorySerializer = new FactorySerializer();
        }

        public static IFactorySerializer FactorySerializer { get; set; }

        #region Public Members

        #region Get

        public static async Task<HttpResponse<TResult>> GetAsync<TResult>(String baseAddress, String requestUri,
            IDictionary<String, IEnumerable<String>> headers = null, ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> GetAsync<TResult, TSettings>(String baseAddress,
            String requestUri, TSettings requestSettings, 
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken)) where TSettings : BaseRequestSettings
        {
            if (requestSettings != null)
            {
                if (requestUri == null) requestUri = "";
                requestUri += "?" + UrlBuilder.Build(requestSettings);
            }
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> GetAsync<TResult>(String baseAddress, String requestUri,
            IEnumerable<KeyValuePair<String, String>> nameValueCollection, 
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            if (nameValueCollection != null)
            {
                if (requestUri == null) requestUri = "";
                requestUri += "?" + UrlBuilder.Build(nameValueCollection);
            }
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<Byte[]>> GetBytesAsync(String baseAddress, String requestUri,
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);

            HttpResponse<Byte[]> httpResponse = new HttpResponse<Byte[]>();

            if (response.IsSuccessStatusCode)
            {
                httpResponse.Result = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                await LoggerHolder.Log(LogLevel.Debug, String.Concat("CONTENT: ", "Byte[] content. Length:", httpResponse.Result.Length));

            }
            else
            {
                var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                await LoggerHolder.Log(LogLevel.Debug, String.Concat("CONTENT: ", stringContent));

                try
                {
                    serializer = serializer ??
                                 FactorySerializer.CreateSerializer(response.Content.Headers.ContentType.MediaType);
                    var error = serializer.Deserialize<ErrorResponse>(stringContent);
                    httpResponse.Errors = error.Errors;
                }
                catch (Exception)
                {
                    httpResponse.Errors = new Dictionary<string, string[]>();
                    httpResponse.Errors.Add(" ", new []{stringContent});
                }
            }

            httpResponse.StatusCode = response.StatusCode;
            return httpResponse;
        }


        #endregion

        #region Post

        public static async Task<HttpResponse<TResult>> PostAsync<TResult>(String baseAddress, String requestUri,
            IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response = await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult, TSettings>(String baseAddress,
            String requestUri, TSettings settings, 
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            var postSerializer = serializer ?? FactorySerializer.CreateSerializer();
            var body = postSerializer.Serialize(settings);
            using (var httpContent = new StringContent(body, Encoding.UTF8, postSerializer.ContentType))
            {
                response = await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token)
                    .ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult>(String baseAddress, String requestUri,
            BytesContent data,
            IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            if (data == null) throw new ArgumentNullException("data");

            HttpResponseMessage response;
            using (var multiPartContent = new MultipartFormDataContent())
            {
                foreach (var parameter in nameValueCollection)
                {
                    var stringContent = new StringContent(WebUtility.UrlDecode(parameter.Value));
                    multiPartContent.Add(stringContent, parameter.Key);
                }

                var imageContent = new ByteArrayContent(data.Bytes);
                if (!String.IsNullOrEmpty(data.ContentType))
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(data.ContentType);
                multiPartContent.Add(imageContent, "file");

                response =
                    await PostBaseAsync(baseAddress, requestUri, multiPartContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult>(String baseAddress, String requestUri,
            BytesContent data, IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            if (data == null) throw new ArgumentNullException("data");
            HttpResponseMessage response;
            using (var multiPartContent = new MultipartFormDataContent())
            {
                var imageContent = new ByteArrayContent(data.Bytes);
                if (!String.IsNullOrEmpty(data.ContentType))
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(data.ContentType);
                multiPartContent.Add(imageContent, "file");
                response =
                    await PostBaseAsync(baseAddress, requestUri, multiPartContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PostAsync<TResult, TSettings>(String baseAddress,
           String requestUri, TSettings settings,
           IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers = null,
           ISerializer serializer = null,
           CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            var putSerializer = serializer ?? FactorySerializer.CreateSerializer();
            var body = putSerializer.Serialize(settings);
            using (var httpContent = new StringContent(body, Encoding.UTF8, putSerializer.ContentType))
            {
                response =
                    await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

            #region Delete

        public static async Task<HttpResponse<TResult>> DeleteAsync<TResult>(String baseAddress, String requestUri,
            IDictionary<String, IEnumerable<String>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            var response = await DeleteBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);
            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #region Put

        public static async Task<HttpResponse<TResult>> PutAsync<TResult>(String baseAddress, String requestUri,
            IEnumerable<KeyValuePair<String, String>> nameValueCollection,
            IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response = await PutBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        public static async Task<HttpResponse<TResult>> PutAsync<TResult, TSettings>(String baseAddress,
            String requestUri,TSettings settings,
            IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers = null,
            ISerializer serializer = null,
            CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            var putSerializer = serializer ?? FactorySerializer.CreateSerializer();
            var body = putSerializer.Serialize(settings);
            using (var httpContent = new StringContent(body, Encoding.UTF8, putSerializer.ContentType))
            {
                response =
                    await PutBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            return await ParseResult<TResult>(serializer, response);
        }

        #endregion

        #endregion

        private static async Task<HttpResponse<TResult>> ParseResult<TResult>(ISerializer serializer, HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await LoggerHolder.Log(LogLevel.Debug, String.Concat("CONTENT: ", stringContent));

            HttpResponse<TResult> httpResponse = new HttpResponse<TResult>();

            serializer = serializer ?? (response.Content.Headers.ContentType != null ? FactorySerializer.CreateSerializer(response.Content.Headers.ContentType.MediaType) : FactorySerializer.CreateSerializer());

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrWhiteSpace(stringContent))
                {
                    httpResponse.Result = serializer.Deserialize<TResult>(stringContent);
                    httpResponse.RawData = stringContent;
                }
            }
            else
            {
                try
                {
                    var error = serializer.Deserialize<ErrorResponse>(stringContent);
                    httpResponse.Errors = error.Errors;
                }
                catch (Exception)
                {
                    httpResponse.Errors = new Dictionary<string, string[]>();
                    httpResponse.Errors.Add(" ", new[] { stringContent });
                }
            }

            httpResponse.StatusCode = response.StatusCode;
            return httpResponse;
        }
    }
}
