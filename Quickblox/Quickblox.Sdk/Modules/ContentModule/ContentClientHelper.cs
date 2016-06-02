using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule.Models;
using Quickblox.Sdk.Modules.ContentModule.Requests;
using Quickblox.Sdk.Modules.ContentModule.Response;

namespace Quickblox.Sdk.Modules.ContentModule
{
    public class ContentClientHelper
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        public ContentClientHelper(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Uploads an image to cloud. 
        /// </summary>
        /// <param name="imageBytes">Image bytes</param>
        /// <param name="isPublic">Is image publicly accessible</param>
        /// <returns>Blob upload info (contains Uid)</returns>
        public async Task<BlobUploadInfo> UploadImage(byte[] imageBytes, bool isPublic)
        {
            var blob = await CreateFileBlob(isPublic);

            if (blob == null)
                return null;

            bool isFileUploaded = await UploadFile(imageBytes, blob.BlobObjectAccess);

            if (!isFileUploaded)
                return null;

            bool isUploadCompleted = await DeclareFileUploaded(imageBytes.Length, blob.Id);

            if (!isUploadCompleted)
                return null;

            return new BlobUploadInfo() { Id = blob.Id, UId = blob.Uid, IsPublic = isPublic };
        }

        /// <summary>
        /// Downloads an image from cloud.
        /// </summary>
        /// <param name="blobUid">Upload blob UID</param>
        /// <returns>Image bytes</returns>
        public async Task<byte[]> DownloadImage(string blobUid)
        {
            var downloadResponse = await quickbloxClient.ContentClient.DownloadFileByUid(blobUid, false);

            if (downloadResponse.StatusCode != HttpStatusCode.OK)
                return null;

            return downloadResponse.Result;
        }

        /// <summary>
        /// Generates a URL for uploaded file blob. You can use this URL to download file.
        /// </summary>
        /// <param name="imageBlobUid">File blob UId</param>
        /// <param name="isPublic">Is file public</param>
        /// <returns>Image URL</returns>
        public string GenerateImageUrl(string imageBlobUid, bool isPublic)
        {
            var imageUrlBuilder = new StringBuilder();
            imageUrlBuilder.Append(quickbloxClient.ApiEndPoint);
            imageUrlBuilder.Append(string.Format(QuickbloxMethods.DownloadFileByUIdMethod, imageBlobUid));
            if (!isPublic)
            {
                imageUrlBuilder.Append($"?token={quickbloxClient.Token}");
            }
            return imageUrlBuilder.ToString();
        }

        #region Depricated methods

        /// <summary>
        /// Uploads publicly accessible image.
        /// </summary>
        /// <param name="imageBytes">Image bytes</param>
        /// <returns>Image link</returns>
        [Obsolete("Use UploadImage method instead.")]
        public async Task<ImageUploadResult> UploadPublicImage(byte[] imageBytes)
        {
            var imageUploadResult = new ImageUploadResult();

            var createFileRequest = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = $"image_{Guid.NewGuid()}.jpeg",
                    IsPublic = true
                }
            };

            var createFileInfoResponse = await quickbloxClient.ContentClient.CreateFileInfoAsync(createFileRequest);

            if (createFileInfoResponse.StatusCode != HttpStatusCode.Created) return null;

            imageUploadResult.BlodId = createFileInfoResponse.Result.Blob.Id;

            var uploadFileRequest = new UploadFileRequest
            {
                BlobObjectAccess = createFileInfoResponse.Result.Blob.BlobObjectAccess,
                FileContent = new BytesContent()
                {
                    Bytes = imageBytes,
                    ContentType = "image/jpg",
                }
            };

            var uploadFileResponse = await quickbloxClient.ContentClient.FileUploadAsync(uploadFileRequest);

            if (uploadFileResponse.StatusCode != HttpStatusCode.Created) return null;

            imageUploadResult.Url = uploadFileResponse.Result.Location;

            var blobUploadCompleteRequest = new BlobUploadCompleteRequest
            {
                BlobUploadSize = new BlobUploadSize() { Size = (uint)imageBytes.Length }
            };
            var response = await quickbloxClient.ContentClient.FileUploadCompleteAsync(createFileInfoResponse.Result.Blob.Id, blobUploadCompleteRequest);

            return imageUploadResult;
        }

        /// <summary>
        /// Uploads privatly accessible image.
        /// </summary>
        /// <param name="imageBytes">Image bytes</param>
        /// <returns>Upload ID</returns>
        [Obsolete("Use UploadImage method instead.")]
        public async Task<int?> UploadPrivateImage(byte[] imageBytes)
        {
            var createFileRequest = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = $"image_{Guid.NewGuid()}.jpeg",
                    IsPublic = false
                }
            };

            var createFileInfoResponse = await quickbloxClient.ContentClient.CreateFileInfoAsync(createFileRequest);

            if (createFileInfoResponse.StatusCode != HttpStatusCode.Created) return null;

            var uploadFileRequest = new UploadFileRequest
            {
                BlobObjectAccess = createFileInfoResponse.Result.Blob.BlobObjectAccess,
                FileContent = new BytesContent()
                {
                    Bytes = imageBytes,
                    ContentType = "image/jpg",
                }
            };

            var uploadFileResponse = await quickbloxClient.ContentClient.FileUploadAsync(uploadFileRequest);

            if (uploadFileResponse.StatusCode != HttpStatusCode.Created) return null;

            var blobUploadCompleteRequest = new BlobUploadCompleteRequest
            {
                BlobUploadSize = new BlobUploadSize() { Size = (uint)imageBytes.Length }
            };
            var response = await quickbloxClient.ContentClient.FileUploadCompleteAsync(createFileInfoResponse.Result.Blob.Id, blobUploadCompleteRequest);

            return createFileInfoResponse.Result.Blob.Id;
        }

        #endregion

        #endregion

        #region Private methods

        private async Task<Blob> CreateFileBlob(bool isPublic)
        {
            var createFileRequest = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = $"image_{Guid.NewGuid()}",
                    IsPublic = isPublic
                }
            };

            var createFileInfoResponse = await quickbloxClient.ContentClient.CreateFileInfoAsync(createFileRequest);

            if (createFileInfoResponse.StatusCode != HttpStatusCode.Created)
                return null;

            return createFileInfoResponse.Result.Blob;
        }

        private async Task<bool> UploadFile(byte[] imageBytes, BlobObjectAccess blobObjectAccess)
        {
            var uploadFileRequest = new UploadFileRequest
            {
                BlobObjectAccess = blobObjectAccess,
                FileContent = new BytesContent()
                {
                    Bytes = imageBytes,
                    ContentType = "image/jpg",
                }
            };

            var uploadFileResponse = await quickbloxClient.ContentClient.FileUploadAsync(uploadFileRequest);

            return uploadFileResponse.StatusCode == HttpStatusCode.Created;
        }

        private async Task<bool> DeclareFileUploaded(int uploadedFileSize, int blobId)
        {
            var blobUploadCompleteRequest = new BlobUploadCompleteRequest
            {
                BlobUploadSize = new BlobUploadSize() { Size = (uint)uploadedFileSize }
            };
            var uploadCompleteResponse = await quickbloxClient.ContentClient.FileUploadCompleteAsync(blobId, blobUploadCompleteRequest);

            return uploadCompleteResponse.StatusCode == HttpStatusCode.OK;
        } 

        #endregion
    }
}
