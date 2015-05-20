using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk.Test.Http
{
    //TODO: write more tests
    [TestClass]
    public class HttpServiceTest
    {
        [TestMethod]
        public async Task GetTest()
        {
            var response = await HttpBase.GetAsync("http://google.com", null);
        }

        [TestMethod]
        public async Task PostTest()
        {
            var response = await HttpBase.PostAsync("http://google.com", null, new List<KeyValuePair<string,string>>());
        }

    }
}
