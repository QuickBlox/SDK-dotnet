using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule.Models;
using Quickblox.Sdk.Modules.ContentModule.Requests;
using Quickblox.Sdk.Modules.ContentModule.Response;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk.Modules.ContentModule
{
    public class ContentClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly IQuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ContentClient(IQuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        /// <summary>
        /// Create an entity which is a file in a system.
        /// </summary>
        /// <param name="createFileRequest">The file parameter.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<FileInfoResponse>> CreateFileInfoAsync(CreateFileRequest createFileRequest)
        {
            
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var createFileResponse = await HttpService.PostAsync<FileInfoResponse, CreateFileRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.CreateFileMethod,
                                                                                                        createFileRequest,
                                                                                                        headers);

            return createFileResponse;
        }

        /// <summary>
        /// Get list of files for the current user. The ID of the user is taken from the token specified in the request.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FilesPagedResponse>> GetFilesAsync()
        {
            
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getFilesResponse = await HttpService.GetAsync<FilesPagedResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.GetFilesMethod,
                                                                                                        headers);
            return getFilesResponse;
        }

        /// <summary>
        /// Get list of tagged files for current user. Will be returned files which have the same tags as current user.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FilesPagedResponse>> GetTaggedFilesAsync()
        {
            
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getTaggedFilesResponse = await HttpService.GetAsync<FilesPagedResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                         QuickbloxMethods.GetTaggedFilesMethod,
                                                                                         headers);
            return getTaggedFilesResponse;
        }

        /// <summary>
        /// Upload a file with the params of BlobObjectAccess info to make a possibility to create items with a content.
        /// </summary>
        /// <param name="uploadFileRequest">The upload file request.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<PostResponse>> FileUploadAsync(UploadFileRequest uploadFileRequest)
        {
            
            var blobObjectAccessParams = uploadFileRequest.BlobObjectAccess.Params;

            String uriMethod = null;
            IEnumerable<KeyValuePair<String, String>> parameters = null;
            try
            {
                var uriAndParameters = blobObjectAccessParams.Split('?');
                uriMethod = uriAndParameters[0];
                parameters = uriAndParameters[1].Split('&')
                    .ToDictionary(key => key.Split('=')[0], value => value.Split('=')[1]);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can't parse BlobObjectAccess parameters" + ex);
            }

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var fileUploadResponse = await HttpService.PostAsync<PostResponse>(uriMethod, String.Empty, uploadFileRequest.FileContent, parameters, headers);
            return fileUploadResponse;
        }

        /// <summary>
        /// Declaring file uploaded. Set file status to 'Complete'. If the specified file size does not match to the actual, the actual will be set.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="blobUploadCompleteRequest">The BLOB upload complete request.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> FileUploadCompleteAsync(Int32  fileId, BlobUploadCompleteRequest blobUploadCompleteRequest)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.CompleteUploadByFileIdMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var fileUploadResponse = await HttpService.PutAsync<Object, BlobUploadCompleteRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        blobUploadCompleteRequest,
                                                                                                        headers);

            return fileUploadResponse;
        }
        
        /// <summary>
        /// Get information about file by ID.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<FileInfoResponse>> GetFileInfoByIdAsync(Int32 fileId)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.GetFileByIdMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var getFilesByIdResponse = await HttpService.GetAsync<FileInfoResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        headers);
            return getFilesByIdResponse;
        }

        /// <summary>
        /// Download File (Get File as a redirect to the S3 object) by uid. 'uid' is a parameter which should be taken from the response of the request for the creating a file. To have a possibility to download a file you should set a status complete to your file firstly.
        /// </summary>
        /// <param name="fileGuid">The file unique identifier.</param>
        /// <returns>Success HTTP Status Code 301</returns>
        public async Task<HttpResponse<Byte[]>> DownloadFileByUid(String fileGuid)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.DownloadFileByUIdMethod, fileGuid);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var downloadFileResponse = await HttpService.GetBytesAsync(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
            return downloadFileResponse;
        }

        /// <summary>
        /// Download File (Get File as a redirect to the S3 object) by Id.
        /// </summary>
        /// <param name="uploadId">UploadId</param>
        /// <returns>Success HTTP Status Code 301</returns>
        public async Task<HttpResponse<Byte[]>> DownloadFileById(int uploadId)
        {
            var uriMethod = String.Format(QuickbloxMethods.DownloadFileByIdMethod, uploadId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var downloadFileResponse = await HttpService.GetBytesAsync(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
            return downloadFileResponse;
        }

        /// <summary>
        /// Get File by ID as BlobObjectAccess with read access. Then we can use info from params element for download file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<ReadOnlyAccessResponse>> GetReadOnlyFileInfoByIdAsync(Int32 fileId)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.GetFileByIdReadOnlyMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var boaResponse = await HttpService.PostAsync<ReadOnlyAccessResponse, GetBlobObjectByIdRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
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
        public async Task<HttpResponse<FileInfoResponse>> EditFileByIdAsync(Int32 fileId, UpdateFileByIdRequest updateFileByIdRequest)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.EditFileMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var editFileById = await HttpService.PutAsync<FileInfoResponse, UpdateFileByIdRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
                                                                                            updateFileByIdRequest,
                                                                                            headers);
            return editFileById;
        }

        /// <summary>
        /// Delete file by ID. If there are some referents to the file the number of links will be reduced by 1 after deleting. A file will be deleted in fact when the number of links will be equal to 0.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteFileAsync(Int32 fileId)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.DeleteFileMethod, fileId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var deleteFileById = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                            uriMethod,
                                                                                            headers);
            return deleteFileById;
        }
    }
}
