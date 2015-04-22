using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.ContentModule.Requests;
using Quickblox.Sdk.Modules.ContentModule.Response;

namespace Quickblox.Sdk.Modules.ContentModule
{
    public class ContentClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly QuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ContentClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        /// <summary>
        /// Create an entity which is a file in a system.
        /// </summary>
        /// <param name="createFileRequest">The file parameter.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<FileResponseInfo>> CreateFile(CreateFileRequest createFileRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createFileResponse = await HttpService.PostAsync<FileResponseInfo, CreateFileRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.CreateFileMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        createFileRequest,
                                                                                                        headers);

            return createFileResponse;
        }

        /// <summary>
        /// Get list of files for the current user. The ID of the user is taken from the token specified in the request.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FilesPagedResponse>> GetFiles()
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getFilesResponse = await HttpService.GetAsync<FilesPagedResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.GetFilesMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        headers);
            return getFilesResponse;
        }

        /// <summary>
        /// Get list of tagged files for current user. Will be returned files which have the same tags as current user.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FilesPagedResponse>> GetTaggedFiles()
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getTaggedFilesResponse = await HttpService.GetAsync<FilesPagedResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                         QuickbloxMethods.GetTaggedFilesMethod,
                                                                                         new NewtonsoftJsonSerializer(),
                                                                                         headers);
            return getTaggedFilesResponse;
        }

        /// <summary>
        /// Upload a file with the params of BlobObjectAccess info to make a possibility to create items with a content.
        /// </summary>
        /// <param name="uploadFileRequest">The upload file request.</param>
        /// <returns></returns>
        public async Task<HttpResponse> FileUpload(UploadFileRequest uploadFileRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var blob = uploadFileRequest.FileInfo.BlobObjectAccess.Params;
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getTaggedFilesResponse = await HttpService.PostAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                         QuickbloxMethods.GetTaggedFilesMethod,
                                                                                         new NewtonsoftJsonSerializer(),
                                                                                         uploadFileRequest.FileContent,
                                                                                         headers);
            return getTaggedFilesResponse;
        }

        /// <summary>
        /// Declaring file uploaded. Set file status to 'Complete'. If the specified file size does not match to the actual, the actual will be set.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="blobUploadCompleteRequest">The BLOB upload complete request.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> FileUploadComplete(Int32  fileId, BlobUploadCompleteRequest blobUploadCompleteRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.CompleteUploadByFileIdMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var fileUploadResponse = await HttpService.PutAsync<Object, BlobUploadCompleteRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        blobUploadCompleteRequest,
                                                                                                        headers);

            return fileUploadResponse;
        }
        
        /// <summary>
        /// Get information about file by ID.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FileResponseInfo>> GetFileInfoById(Int32 fileId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.GetFileByIdMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getFilesByIdResponse = await HttpService.GetAsync<FileResponseInfo>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        headers);
            return getFilesByIdResponse;
        }

        /// <summary>
        /// Download File (Get File as a redirect to the S3 object) by uid. 'uid' is a parameter which should be taken from the response of the request for the creating a file. To have a possibility to download a file you should set a status complete to your file firstly.
        /// </summary>
        /// <param name="fileGuid">The file unique identifier.</param>
        /// <returns>Success HTTP Status Code 301</returns>
        public async Task<HttpResponse<Byte[]>> DownloadFile(String fileGuid)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DownloadFileByIdMethod, fileGuid);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var downloadFileResponse = await HttpService.GetAsync<Byte[]>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        headers);
            return downloadFileResponse;
        }

        /// <summary>
        /// Get File by ID as BlobObjectAccess with read access. Then we can use info from params element for download file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<ReadOnlyAccessResponse>> GetReadOnlyFileInfoById(Int32 fileId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.GetFileByIdReadOnlyMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var boaResponse = await HttpService.PostAsync<ReadOnlyAccessResponse, GetBlobObjectByIdRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
                                                                                            new NewtonsoftJsonSerializer(),
                                                                                            new GetBlobObjectByIdRequest(),
                                                                                            headers);
            return boaResponse;
        }

        /// <summary>
        /// Edit a file by ID.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="updateFileByIdRequest">The update file request parameter.</param>
        /// <returns></returns>
        public async Task<HttpResponse<FileResponseInfo>> EditFileById(Int32 fileId, UpdateFileByIdRequest updateFileByIdRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.EditFileMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var editFileById = await HttpService.PutAsync<FileResponseInfo, UpdateFileByIdRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
                                                                                            new NewtonsoftJsonSerializer(),
                                                                                            updateFileByIdRequest,
                                                                                            headers);
            return editFileById;
        }

        /// <summary>
        /// Delete file by ID. If there are some referents to the file the number of links will be reduced by 1 after deleting. A file will be deleted in fact when the number of links will be equal to 0.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteFile(Int32 fileId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteFileMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var deleteFileById = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
                                                                                            new NewtonsoftJsonSerializer(),
                                                                                            headers);
            return deleteFileById;
        }
    }
}
