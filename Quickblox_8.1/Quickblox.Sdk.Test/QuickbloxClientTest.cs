using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;

namespace Quickblox.Sdk.Test
{
    [TestClass]
    public class QuickbloxClientTest
    {
        [TestMethod]
        public async Task TestInitialization()
        {
            //Quickblox.Sdk.QuickbloxClient client = new QuickbloxClient();
            //await client.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());

            //Assert.IsTrue(client.IsClientInitialized);
        }

        [TestMethod]
        public async Task TestFailInitialization()
        {
            //Quickblox.Sdk.QuickbloxClient client = new QuickbloxClient();

            //Assert.IsFalse(client.IsClientInitialized);
            //var ex = AssertEx.ThrowsAsync<NotInitializedException>(
            //    async () =>
            //    {
            //        await client.CoreClient.CreateSessionBaseAsync(0, "", "");
            //    });
        }
    }
}
