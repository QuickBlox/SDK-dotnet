using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;
using Quickblox.Sdk.Modules.CustomObjectModule.Requests;
using Quickblox.Sdk.Modules.CustomObjectModule.Responses;

namespace Quickblox.Sdk.Modules.CustomObjectModule
{
    public class CustomObjectsClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly QuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomObjectsClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public CustomObjectsClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> RetriveCustomObjectsByIdsAsync<T>(
            String className, String ids) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (ids == null) throw new ArgumentNullException("ids");
            if (className == null) throw new ArgumentNullException("className");

            var requestUri = String.Format(QuickbloxMethods.RetriveObjectsByIdsMethod, className, ids);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createFileResponse =
                await HttpService.GetAsync<RetriveCustomObjectsResponce<T>>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    headers);
            return createFileResponse;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> RetriveCustomObjectsAsync<T>(String className)
            where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");

            var requestUri = String.Format(QuickbloxMethods.RetriveObjectsMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createFileResponse =
                await HttpService.GetAsync<RetriveCustomObjectsResponce<T>>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    headers);
            return createFileResponse;
        }

        public async Task<HttpResponse<T>> CreateCustomObjectsAsync<T>(String className,
            CreateCustomObjectRequest<T> customObject) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");
            if (customObject == null) throw new ArgumentNullException("customObject");

            var requestUri = String.Format(QuickbloxMethods.CreateCustomObjectMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var createFileResponse =
                await HttpService.PostAsync<T, CreateCustomObjectRequest<T>>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    customObject,
                    headers);
            return createFileResponse;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> CreateMultiCustomObjectsAsync<T>(
            String className, List<T> items) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");
            if (items == null) throw new ArgumentNullException("items");

            var requestUri = String.Format(QuickbloxMethods.CreateMultiCustomObjectMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var nameValueCollection = new List<KeyValuePair<String, String>>();
            for (int itemIndex = 0; itemIndex < items.Count; itemIndex++)
            {
                var properties = items[itemIndex].GetType().GetRuntimeProperties();
                foreach (var property in properties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null))
                {
                    var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();

                    var pair =
                        new KeyValuePair<string, string>(
                            String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName),
                            property.GetValue(items[itemIndex]).ToString());
                    nameValueCollection.Add(pair);
                }
            }

            var createMultiCustomObjects =
                await HttpService.PostAsync<RetriveCustomObjectsResponce<T>>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    nameValueCollection,
                    headers);
            return createMultiCustomObjects;
        }

        public async Task<HttpResponse<T>> UpdateCustomObjectsByIdAsync<T>(String className,
            UpdateCustomObjectRequest<T> updateCustomObjectRequest) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");
            if (updateCustomObjectRequest == null) throw new ArgumentNullException("updateCustomObjectRequest");

            var requestUri = String.Format(QuickbloxMethods.UpdateCustomObjectMethod, className, updateCustomObjectRequest.CustomObject.Id);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var updateCustomObject = await HttpService.PutAsync<T, T>(this.quickbloxClient.ApiEndPoint,
                requestUri,
                updateCustomObjectRequest.CustomObject,
                headers);
            return updateCustomObject;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> UpdateMultiCustomObjectsAsync<T>(
            String className, List<T> items) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");

            var requestUri = String.Format(QuickbloxMethods.UpdateMultiCustomObjectMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var nameValueCollection = new List<KeyValuePair<String, String>>();
            for (int itemIndex = 0; itemIndex < items.Count; itemIndex++)
            {
                var properties = items[itemIndex].GetType().GetRuntimeProperties();
                foreach (var property in properties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null))
                {
                    var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();

                    var pair =
                        new KeyValuePair<string, string>(
                            String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName),
                            property.GetValue(items[itemIndex]).ToString());
                    nameValueCollection.Add(pair);
                }
            }

            var createMultiCustomObjects =
                await HttpService.PutAsync<RetriveCustomObjectsResponce<T>>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    nameValueCollection,
                    headers);
            return createMultiCustomObjects;
        }

        public async Task<HttpResponse> DeleteCustomObjectsByIdAsync<T>(String className, String id) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");
            if (id == null) throw new ArgumentNullException("id");

            var requestUri = String.Format(QuickbloxMethods.DeleteCustomObjectMethod, className, id);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var deleteCustomObjectRequest =
                await HttpService.DeleteAsync<HttpResponse>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    headers);
            return deleteCustomObjectRequest;
        }

        public async Task<HttpResponse> DeleteCustomObjectsByIdsAsync<T>(String className, String ids) where T : BaseCustomObject
        {
            this.quickbloxClient.CheckIsInitialized();

            if (className == null) throw new ArgumentNullException("className");
            if (ids == null) throw new ArgumentNullException("ids");

            var requestUri = String.Format(QuickbloxMethods.DeleteCustomObjectMethod, className, ids);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var deleteCustomObjectRequest =
                await HttpService.DeleteAsync<HttpResponse>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    headers);
            return deleteCustomObjectRequest;
        }
    }
}
