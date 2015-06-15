using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule.Models;
using Quickblox.Sdk.Modules.ContentModule.Requests;
using Quickblox.Sdk.Test.Helper;

namespace Quickblox.Sdk.Test.Modules.ContentModule
{
    [TestClass]
    public class ContentClientTest
    {
        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient();
            await this.client.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await
                this.client.CoreClient.CreateSessionWithLoginAsync(GlobalConstant.ApplicationId,
                    GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345",
                    deviceRequestRequest:
                        new DeviceRequest() {Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId()});
        }

        [TestMethod]
        public async Task CreateFileInfoSuccessTest()
        {
            var settings = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = "museum.jpeg",
                }
            };

            var createFileInfoResponse = await this.client.ContentClient.CreateFileInfoAsync(settings);
            Assert.AreEqual(createFileInfoResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetFilesInfoSuccessTest()
        {
            var getFilesResponse = await this.client.ContentClient.GetFilesAsync();
            Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetTaggedFilesInfoSuccessTest()
        {
            var getFilesResponse = await this.client.ContentClient.GetTaggedFilesAsync();
            Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task FileUploadSuccessTest()
        {
            var settings = new CreateFileRequest()
            {
                Blob = new BlobRequest()
                {
                    Name = String.Format("museum_{0}.jpeg", Guid.NewGuid()),
                }
            };

            var createFileInfoResponse = await this.client.ContentClient.CreateFileInfoAsync(settings);
            Assert.AreEqual(createFileInfoResponse.StatusCode, HttpStatusCode.Created);

            var uploadFileRequest = new UploadFileRequest();
            uploadFileRequest.BlobObjectAccess = createFileInfoResponse.Result.Blob.BlobObjectAccess;

            var uri = new Uri("ms-appx:///Modules/ContentModule/Assets/1.jpg");
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var stream = await storageFile.OpenReadAsync();
            var bytes = new byte[stream.Size];

            using (var dataReader = new DataReader(stream))
            {
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(bytes);
            }

            uploadFileRequest.FileContent = new BytesContent()
            {
                Bytes = bytes,
                ContentType = "image/jpg",
            };

            var uploadFileResponse = await this.client.ContentClient.FileUploadAsync(uploadFileRequest);
            Assert.AreEqual(uploadFileResponse.StatusCode, HttpStatusCode.Created);

            var blobUploadCompleteRequest = new BlobUploadCompleteRequest();
            blobUploadCompleteRequest.BlobUploadSize = new BlobUploadSize() { Size = (uint) bytes.Length };
            var uploadFileCompleteResponse = await this.client.ContentClient.FileUploadCompleteAsync(createFileInfoResponse.Result.Blob.Id, blobUploadCompleteRequest);
            Assert.AreEqual(uploadFileCompleteResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteFilesSuccessTest()
        {
            var getFilesResponse = await this.client.ContentClient.GetFilesAsync();
            Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);

            foreach (var item in getFilesResponse.Result.Items)
            {
                var deleteResponse = await client.ContentClient.DeleteFileAsync(item.Blob.Id);
                Assert.AreEqual(deleteResponse.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task DownloadFileSuccessTest()
        {
            //var getFilesResponse = await this.client.ContentClient.GetFilesAsync();
            //Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);
            //var firstFile = getFilesResponse.Result.Items.Last();

            var downloadFileResponse = await this.client.ContentClient.DownloadFileAsync("35f6c9cb777340d989ba01770bcc4e2000");
            Assert.AreEqual(downloadFileResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DownloadFileFailTest()
        {
            var getFilesResponse = await this.client.ContentClient.GetFilesAsync();
            Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);
            var firstFile = getFilesResponse.Result.Items.Last();

            var downloadFileResponse = await this.client.ContentClient.DownloadFileAsync(firstFile.Blob.Uid);
            Assert.IsNotNull(downloadFileResponse.Errors);
        }
    }
}
