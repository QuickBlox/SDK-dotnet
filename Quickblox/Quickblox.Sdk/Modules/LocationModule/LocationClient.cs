using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.LocationModule.Requests;
using Quickblox.Sdk.Modules.LocationModule.Responses;

namespace Quickblox.Sdk.Modules.LocationModule
{
    /// <summary>
    /// Client present API for push
    /// http://quickblox.com/developers/Location
    /// </summary>
    public class LocationClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly QuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        internal LocationClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }


        /// <summary>
        /// Create geodata which represent points on the earth
        /// </summary>
        /// <param name="createGeodataRequest">The create geodata request.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<GeoDatumResponse>> CreateGeoDataAsync(CreateGeoDataRequest createGeoDataRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createGeodataResponse = await HttpService.PostAsync<GeoDatumResponse, CreateGeoDataRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.CreateGeoDataMethod,
                                                                                                        createGeoDataRequest,
                                                                                                        headers);

            return createGeodataResponse;
        }

        /// <summary>
        /// You can create or update geodata with sending a push notification to all users who is located in your radius.
        /// </summary>
        /// <param name="createGeoDataWithPushRequest">The create geo data with push request.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<GeoDataResponse>> CreatePushGeoDataAsync(CreateGeoDataWithPushRequest createGeoDataWithPushRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createGeodataResponse = await HttpService.PostAsync<GeoDataResponse, CreateGeoDataWithPushRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.CreateGeoDataMethod,
                                                                                                        createGeoDataWithPushRequest,
                                                                                                        headers);
            return createGeodataResponse;
        }

        /// <summary>
        ///Update geodata
        /// </summary>
        /// <param name="geoDataId">The geo data identifier.</param>
        /// <param name="updateGeoDataRequest">The update geo data with push request.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GeoDatumResponse>> UpdateGeoDataAsync(Int32 geoDataId, UpdateGeoDataRequest updateGeoDataRequest)
        {
            var uri = String.Format(QuickbloxMethods.UpdateByIdGeoDataMethod, geoDataId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var updateGeoDataResponse = await HttpService.PutAsync<GeoDatumResponse, UpdateGeoDataRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uri,
                                                                                                        updateGeoDataRequest,
                                                                                                        headers);
            return updateGeoDataResponse;
        }

        /// <summary>
        /// Retrieve geodata by the identifier
        /// </summary>
        /// <param name="geoDataId">The geo data identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GeoDatumWithUserResponse>> GetGeoDataByIdAsync(Int32 geoDataId)
        {
            var uri = String.Format(QuickbloxMethods.GetByIdGeoDataMethod, geoDataId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getGeoDataResponse = await HttpService.GetAsync<GeoDatumWithUserResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uri,
                                                                                                        headers);
            return getGeoDataResponse;
        }

        /// <summary>
        /// Retrieve all (by default) geodata for current application. The ID of the application is taken from the token which is specified in the request
        /// </summary>
        /// <param name="findGeoDataRequest">
        /// Filters
        /// The request can contain all, some or none of these parameters.If this option is set,  its value - the object to validate.
        /// Filters require an exact match property values ​​with an instance of the corresponding parameter value.
        /// </param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<PagedResponse<GeoDatumWithUserResponse>>> FindGeoDataAsync(FindGeoDataRequest findGeoDataRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getGeoDataResponse = await HttpService.GetAsync<PagedResponse<GeoDatumWithUserResponse>, FindGeoDataRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.FindGeoDataMethod,
                                                                                                        findGeoDataRequest,
                                                                                                        headers);
            return getGeoDataResponse;
        }

        /// <summary>
        /// Delete geodata by the identifier
        /// </summary>
        /// <param name="geoDataId">The geo data identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> DeleteGeoDataById(Int32 geoDataId)
        {
            var uri = String.Format(QuickbloxMethods.DeleteByIdGeoDataMethod, geoDataId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getGeoDataResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                        uri,
                                                                        headers);
            return getGeoDataResponse;
        }

        /// <summary>
        /// Maximum age of data that should remain in the database after a query.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> DeleteGeoDataByDays(Int32 days)
        {
            var uri = String.Format(QuickbloxMethods.DeleteGeoWithDaysDataMethod, days);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getGeoDataResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                        uri,
                                                                        headers);
            return getGeoDataResponse;
        }
    }
}
