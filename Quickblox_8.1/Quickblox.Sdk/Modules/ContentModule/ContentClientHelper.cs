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
        public async Task<string> UploadPublicImage(byte[] imageBytes)
        {
            var createFileRequest = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = String.Format("image_{0}.jpeg", Guid.NewGuid()),
                    IsPublic = true
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

            return uploadFileResponse.Result.Location;
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
                    Name = String.Format("image_{0}.jpeg", Guid.NewGuid()),
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
