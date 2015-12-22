using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule.Models;
using Quickblox.Sdk.Modules.ContentModule.Requests;

namespace Quickblox.Sdk.Modules.ContentModule
{
    public class ContentClientHelper
    {
        private readonly ContentClient contentClient;

        public ContentClientHelper(ContentClient contentClient)
        {
            this.contentClient = contentClient;
        }

        /// <summary>
        /// Uploads publicly accessible image.
        /// </summary>
        /// <param name="imageBytes">Image bytes</param>
        /// <returns>Image link</returns>
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

            var createFileInfoResponse = await contentClient.CreateFileInfoAsync(createFileRequest);

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

            var uploadFileResponse = await contentClient.FileUploadAsync(uploadFileRequest);

            if (uploadFileResponse.StatusCode != HttpStatusCode.Created) return null;

            imageUploadResult.Url = uploadFileResponse.Result.Location;

            var blobUploadCompleteRequest = new BlobUploadCompleteRequest
            {
                BlobUploadSize = new BlobUploadSize() { Size = (uint)imageBytes.Length }
            };
            var response = await contentClient.FileUploadCompleteAsync(createFileInfoResponse.Result.Blob.Id, blobUploadCompleteRequest);

            return imageUploadResult;
        }

        /// <summary>
        /// Uploads privatly accessible image.
        /// </summary>
        /// <param name="imageBytes">Image bytes</param>
        /// <returns>Upload ID</returns>
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

            var createFileInfoResponse = await contentClient.CreateFileInfoAsync(createFileRequest);

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

            var uploadFileResponse = await contentClient.FileUploadAsync(uploadFileRequest);

            if (uploadFileResponse.StatusCode != HttpStatusCode.Created) return null;

            var blobUploadCompleteRequest = new BlobUploadCompleteRequest
            {
                BlobUploadSize = new BlobUploadSize() { Size = (uint)imageBytes.Length }
            };
            var response = await contentClient.FileUploadCompleteAsync(createFileInfoResponse.Result.Blob.Id, blobUploadCompleteRequest);

            return createFileInfoResponse.Result.Blob.Id;
        }
    }
}
