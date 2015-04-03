using System;
using System.Collections.Generic;

namespace Quickblox.Sdk.Builder
{
    public static class RequestHeadersBuilder
    {

        public static IDictionary<String, IEnumerable<String>> GetDefaultHeaders()
        {
            return new Dictionary<String, IEnumerable<String>>
            {
                {"QuickBlox-REST-API-Version", new[] {"0.1.1"}}
            };
        }

        public static IDictionary<String, IEnumerable<String>> GetHeaderWithQbAccountKey(this IDictionary<String, IEnumerable<String>> headers, String key)
        {
            headers.Add("QB-Account-Key", new[] {key});
            return headers;
        }

        public static IDictionary<String, IEnumerable<String>> GetHeaderWithQbToken(this IDictionary<String, IEnumerable<String>> headers, String token)
        {
            headers.Add("QB-Token", new[] { token });
            return headers;
        }
    }
}
