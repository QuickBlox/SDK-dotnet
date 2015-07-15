using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quickblox.Sdk.Common;
using Quickblox.Sdk.GeneralDataModel.Response;

namespace Quickblox.Sdk.Http
{
    public class HttpBase
    {
        #region Fields

        private static long ticks;

        #endregion

        #region Properties

        /// <summary>
        /// Возварщает время последнего запроса в UTC.
        /// </summary>
        public static DateTime LastRequest
        {
            get { return new DateTime(ticks); }
        }

        #endregion

        #region Public Members

        public static async Task<String> GetAsync(String baseAddress, String requestUri, IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken))
        {
            var response = await GetBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);

            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await QuickbloxLogger.Instance.Log(LogLevel.Debug, "GET RESPONSE CONTENT: " + stringContent);
            return stringContent;
        }

        public static async Task<String> PostAsync(String baseAddress, String requestUri, IEnumerable<KeyValuePair<String, String>> nameValueCollection, IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response = await PostBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await QuickbloxLogger.Instance.Log(LogLevel.Debug, "POST RESPONSE CONTENT: " + stringContent);
            return stringContent;
        }

        public static async Task<String> PostAsync(String baseAddress, String requestUri, Byte[] data, IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken))
        {
            HttpResponseMessage response;
            using (var multiPartContent = new MultipartFormDataContent())
            {
                var imageContent = new ByteArrayContent(data);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                multiPartContent.Add(imageContent, "image", "image.jpg");
                response = await PostBaseAsync(baseAddress, requestUri, multiPartContent, headers, token).ConfigureAwait(false);
            }

            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await QuickbloxLogger.Instance.Log(LogLevel.Debug, "POST RESPONSE CONTENT: " + stringContent);
            return stringContent;

        }

        public static async Task<String> DeleteAsync(String baseAddress, String requestUri, IDictionary<String, IEnumerable<String>> headers = null, CancellationToken token = default(CancellationToken))
        {
            var response = await DeleteBaseAsync(baseAddress, requestUri, headers, token).ConfigureAwait(false);

            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await QuickbloxLogger.Instance.Log(LogLevel.Debug, "DELETE RESPONSE CONTENT: " + stringContent);
            return stringContent;
        }

        private static async Task<String> PutAsync(String baseAddress, String requestUri, IEnumerable<KeyValuePair<String, String>> nameValueCollection, IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token)
        {
            HttpResponseMessage response;
            using (var httpContent = new FormUrlEncodedContent(nameValueCollection))
            {
                response = await PutBaseAsync(baseAddress, requestUri, httpContent, headers, token).ConfigureAwait(false);
            }

            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            await QuickbloxLogger.Instance.Log(LogLevel.Debug, "PUT RESPONSE CONTENT: " + stringContent);
            return stringContent;
        }

        #endregion

        #region Protected Members

        protected static async Task<HttpResponseMessage> GetBaseAsync(String baseAddress, String requestUri,
            IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token)
        {
            HttpResponseMessage response;
            
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                    }
                    else
                    {
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                    }
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true
                    };
                    client.DefaultRequestHeaders.SetRequestHeaders(headers);


                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, string.Format("==> GET REQUEST: {0}{1}", baseAddress, requestUri));
                    try
                    {
                        response = await client.GetAsync(requestUri, token).ConfigureAwait(false);
                    }
                    catch (HttpRequestException)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    catch (Exception ex)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, string.Format("<== GET RESPONSE: {0}", response));
                    Interlocked.Exchange(ref ticks, DateTime.UtcNow.Ticks);
                }
            }
            return response;
        }

        protected static async Task<HttpResponseMessage> PostBaseAsync(String baseAddress, String requestUri, HttpContent content, IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token)
        {
            HttpResponseMessage response;
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                    }
                    else
                    {
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                    }
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
                    client.DefaultRequestHeaders.SetRequestHeaders(headers);

                    var stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(String.Concat("==> POST REQUEST: ", baseAddress, requestUri));
                    stringBuilder.AppendLine(String.Concat("==> POST HEADERS: ", client.DefaultRequestHeaders));
                    stringBuilder.AppendLine(String.Concat("==> POST CONTENT: ", await content.ReadAsStringAsync()));

                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, stringBuilder.ToString());
                    try
                    {
                        response = await client.PostAsync(requestUri, content, token).ConfigureAwait(false);
                    }
                    catch (HttpRequestException)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    catch (Exception ex)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, String.Concat("<== POST RESPONSE: ", response));
                    Interlocked.Exchange(ref ticks, DateTime.UtcNow.Ticks);
                }
            }
            return response;
        }

        protected static async Task<HttpResponseMessage> DeleteBaseAsync(String baseAddress, String requestUri, IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token)
        {
            HttpResponseMessage response;
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                    }
                    else
                    {
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                    }
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
                    client.DefaultRequestHeaders.SetRequestHeaders(headers);

                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, String.Concat("==> DELETE REQUEST: ", baseAddress, requestUri));
                    try
                    {
                        response = await client.DeleteAsync(requestUri, token).ConfigureAwait(false);
                    }
                    catch (HttpRequestException)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    catch (Exception ex)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, String.Concat("<== DELETE RESPONSE: ", response));
                    Interlocked.Exchange(ref ticks, DateTime.UtcNow.Ticks);
                }
            }
            return response;
        }

        protected static async Task<HttpResponseMessage> PutBaseAsync(String baseAddress, String requestUri, HttpContent content, IEnumerable<KeyValuePair<String, IEnumerable<String>>> headers, CancellationToken token)
        {
            HttpResponseMessage response;
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseAddress);
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                    }
                    else
                    {
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                    }
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
                    client.DefaultRequestHeaders.SetRequestHeaders(headers);

                    string logMessage = string.Format("==> PUT REQUEST: {0}{1} {2} ==> PUT CONTENT: {3}", baseAddress, requestUri, Environment.NewLine, await content.ReadAsStringAsync());
                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, logMessage);

                    try
                    {
                        response = await client.PutAsync(requestUri, content, token).ConfigureAwait(false);
                    }
                    catch (HttpRequestException)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    catch (Exception ex)
                    {
                        response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent("Error internet connection");
                    }
                    await QuickbloxLogger.Instance.Log(LogLevel.Debug, String.Concat("<== PUT RESPONSE: ", response));
                    Interlocked.Exchange(ref ticks, DateTime.UtcNow.Ticks);
                }
            }
            return response;
        }

        #endregion
    }
}
