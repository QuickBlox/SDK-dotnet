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
using System.Collections;
using System.Text;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.CustomObjectModule
{
    public class CustomObjectsClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly IQuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomObjectsClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        internal CustomObjectsClient(IQuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> RetriveCustomObjectsByIdsAsync<T>(
            String className, String ids) where T : BaseCustomObject
        {
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

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> RetriveCustomObjectsAsync<T>(String className, RetriveCustomObjectsWithFilter filter = null)
            where T : BaseCustomObject
        {
            if (className == null) throw new ArgumentNullException("className");

            var requestUri = String.Format(QuickbloxMethods.RetriveObjectsMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createFileResponse =
                await HttpService.GetAsync<RetriveCustomObjectsResponce<T>, RetriveCustomObjectsWithFilter>(this.quickbloxClient.ApiEndPoint,
                    requestUri,
                    filter,
                    headers);
            return createFileResponse;
        }

        public async Task<HttpResponse<T>> CreateCustomObjectsAsync<T>(String className,
            CreateCustomObjectRequest<T> customObject) where T : BaseCustomObject
        {
            if (className == null) throw new ArgumentNullException("className");
            if (customObject == null) throw new ArgumentNullException("customObject");

            var requestUri = String.Format(QuickbloxMethods.CreateCustomObjectMethod, className);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            //var nameValueCollection = new List<KeyValuePair<String, String>>();
            //var properties = customObject.CreateCustomObject.GetType().GetRuntimeProperties();
            //foreach (var property in properties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null))
            //{
            //    var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();

            //    var propertyValue = property.GetValue(customObject.CreateCustomObject);
            //    if (propertyValue != null)
            //    {
            //        if (property.PropertyType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IList)))
            //        {
            //            var items = propertyValue as IEnumerable;
            //            var enumerator = items.GetEnumerator();

            //            StringBuilder stringBuilder = new StringBuilder();
            //            while (enumerator.MoveNext())
            //            {
            //                stringBuilder.Append(enumerator.Current + ",");
            //            }
                        
            //            var pair = new KeyValuePair<string, string>(jsonProperty.PropertyName, stringBuilder.ToString());
            //            nameValueCollection.Add(pair);
            //        }
            //        else
            //        {
            //            var pair = new KeyValuePair<string, string>(jsonProperty.PropertyName, propertyValue.ToString());
            //            nameValueCollection.Add(pair);
            //        }
            //    }

            //}

            //if (customObject.Filter != null)
            //{
            //    requestUri += "?" + UrlBuilder.BuildFilter((FilterAggregator)customObject.Filter);
            //}

            //var updateCustomObject = await HttpService.PutAsync<T, T>(this.quickbloxClient.ApiEndPoint,
            //   requestUri,
            //   customObject.CreateCustomObject,
            //   headers);

            var createFileResponse =
                await HttpService.PostAsync<T, T>(quickbloxClient.ApiEndPoint,
                    requestUri,
                    customObject.CreateCustomObject,
                    headers);
            return createFileResponse;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> CreateMultiCustomObjectsAsync<T>(
            String className, List<T> items) where T : BaseCustomObject
        {     
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

                    var propertyValue = property.GetValue(items[itemIndex]);
                    if (propertyValue != null)
                    {
                        if (property.PropertyType.GetTypeInfo().ImplementedInterfaces.Contains(typeof (IList)))
                        {
                            var propertyFields = propertyValue as IEnumerable;
                            var enumerator = propertyFields.GetEnumerator();

                            StringBuilder stringBuilder = new StringBuilder();
                            while (enumerator.MoveNext())
                            {
                                stringBuilder.Append(enumerator.Current + ",");
                            }

                            var pair = new KeyValuePair<string, string>(String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName), stringBuilder.ToString());
                            nameValueCollection.Add(pair);
                        }
                        else
                        {
                            var pair = new KeyValuePair<string, string>(String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName), property.GetValue(items[itemIndex]).ToString());
                            nameValueCollection.Add(pair);
                        }
                    }
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
                    if (String.Equals(jsonProperty.PropertyName, "_id"))
                    {
                        // in update method we need to use "id" without "_"
                        var pair = new KeyValuePair<string, string>(String.Format("record[{0}][{1}", itemIndex, "id"), property.GetValue(items[itemIndex]).ToString());
                        nameValueCollection.Add(pair);
                        continue;
                    }

                    var propertyValue = property.GetValue(items[itemIndex]);
                    if (propertyValue != null)
                    {
                        if (property.PropertyType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IList)))
                        {
                            var propertyFields = propertyValue as IEnumerable;
                            var enumerator = propertyFields.GetEnumerator();

                            StringBuilder stringBuilder = new StringBuilder();
                            while (enumerator.MoveNext())
                            {
                                stringBuilder.Append(enumerator.Current + ",");
                            }

                            var pair = new KeyValuePair<string, string>(String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName), stringBuilder.ToString());
                            nameValueCollection.Add(pair);
                        }
                        else
                        {
                            var pair = new KeyValuePair<string, string>(String.Format("record[{0}][{1}", itemIndex, jsonProperty.PropertyName), property.GetValue(items[itemIndex]).ToString());
                            nameValueCollection.Add(pair);
                        }
                    }
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

        public async Task<HttpResponse<T>> CreateRelationObjectAsync<T>(String parentClassName, String parentId,  String childClassName, CreateCustomObjectRequest<T> createCustomObjectRequest) where T : BaseCustomObject
        {
            

            if (parentClassName == null) throw new ArgumentNullException("parentClassName");
            if (childClassName == null) throw new ArgumentNullException("childClassName");
            if (parentId == null) throw new ArgumentNullException("parentId");
            if (createCustomObjectRequest == null) throw new ArgumentNullException("createCustomObjectRequest");
            if (createCustomObjectRequest.CreateCustomObject == null) throw new ArgumentNullException("createCustomObjectRequest.CreateCustomObject");

            var requestUri = String.Format(QuickbloxMethods.CreateRelationMethod, parentClassName, parentId, childClassName);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var nameValueCollection = new List<KeyValuePair<String, String>>();
            var properties = createCustomObjectRequest.CreateCustomObject.GetType().GetRuntimeProperties();
            foreach (var property in properties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null))
            {
                var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();

                var propertyValue = property.GetValue(createCustomObjectRequest.CreateCustomObject);
                if (propertyValue != null)
                {
                    if (property.PropertyType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IList)))
                    {
                        var items = propertyValue as IEnumerable;
                        var enumerator = items.GetEnumerator();

                        StringBuilder stringBuilder = new StringBuilder();
                        while (enumerator.MoveNext())
                        {
                            stringBuilder.Append(enumerator.Current + ",");
                        }

                        var pair = new KeyValuePair<string, string>(jsonProperty.PropertyName, stringBuilder.ToString());
                        nameValueCollection.Add(pair);
                    }
                    else
                    {
                        var pair = new KeyValuePair<string, string>(jsonProperty.PropertyName, propertyValue.ToString());
                        nameValueCollection.Add(pair);
                    }
                }

            }

            var createRelationResponse = await HttpService.PostAsync<T>(this.quickbloxClient.ApiEndPoint,
                                                                        requestUri,
                                                                        nameValueCollection,
                                                                        headers);
            return createRelationResponse;
        }

        public async Task<HttpResponse<RetriveCustomObjectsResponce<T>>> RetriveRelationObjectsAsync<T>(String parentClassName, String parentId, String childClassName) where T : BaseCustomObject
        {
            if (parentClassName == null) throw new ArgumentNullException("parentClassName");
            if (childClassName == null) throw new ArgumentNullException("childClassName");
            if (parentId == null) throw new ArgumentNullException("parentId");

            var requestUri = String.Format(QuickbloxMethods.GetRelatedObjectsMethod, parentClassName, parentId, childClassName);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var createRelationResponse = await HttpService.GetAsync<RetriveCustomObjectsResponce<T>>(this.quickbloxClient.ApiEndPoint,
                                                                        requestUri,
                                                                        headers);
            return createRelationResponse;
        }

        /// <summary>
        /// Customs get request for private API
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public async Task<HttpResponse<TResult>> CustomGetRequest<TResult>(String requestUri)
        {
            if (requestUri == null) throw new ArgumentNullException("requestUri");
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var customGetResponse = await HttpService.GetAsync<TResult>(this.quickbloxClient.ApiEndPoint,
                                                                       requestUri,
                                                                       headers);
            return customGetResponse;
        }

        /// <summary>
        /// Customs post request for private API
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TSettings">The type of the settings.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public async Task<HttpResponse<TResult>> CustomPostRequest<TResult, TSettings>(String requestUri, TSettings settings) where TSettings : BaseRequestSettings
        {
            if (requestUri == null) throw new ArgumentNullException("requestUri");
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);

            var customPostResponse = await HttpService.PostAsync<TResult, TSettings>(this.quickbloxClient.ApiEndPoint,
                                                                                                       requestUri,
                                                                                                       settings,
                                                                                                       headers);
            return customPostResponse;
        }
    }
}
