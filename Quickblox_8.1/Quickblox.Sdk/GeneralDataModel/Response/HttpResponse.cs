using System;
using System.Collections.Generic;
using System.Net;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class HttpResponse
    {
        public String RawData { get; set; }

        public Dictionary<string, string[]> Errors { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class HttpResponse<TResult> : HttpResponse
    {
        public TResult Result { get; set; }
    }
}
