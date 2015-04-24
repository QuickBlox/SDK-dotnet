using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Hmacsha;
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
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey,
                new HmacSha1CryptographicProvider());
            await this.client.InitializeClient();
            await
                this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId,
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

            var createFileInfoResponse = await this.client.ContentClient.CreateFileInfo(settings);
            Assert.AreEqual(createFileInfoResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetFilesInfoSuccessTest()
        {
            var getFilesResponse = await this.client.ContentClient.GetFiles();
            Assert.AreEqual(getFilesResponse.StatusCode, HttpStatusCode.OK);
        }
    }
}
