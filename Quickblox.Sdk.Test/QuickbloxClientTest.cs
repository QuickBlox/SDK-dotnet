using System.Threading.Tasks;
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
            Quickblox.Sdk.QuickbloxClient client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await client.InitializeClient();

            Assert.IsTrue(client.IsClientInitialized);
        }

        [TestMethod]
        public async Task TestFailInitialization()
        {
            Quickblox.Sdk.QuickbloxClient client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());

            Assert.IsFalse(client.IsClientInitialized);
            var ex = AssertEx.ThrowsAsync<NotInitializedException>(
                async () =>
                {
                    await client.CoreClient.CreateSessionBase("", "", "");
                });
        }
    }
}
