using System;
using System.Net;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class HttpResponse
    {
        public String RawData { get; set; }

        public Error Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class HttpResponse<TResult> : HttpResponse
    {
        public TResult Result { get; set; }
    }
}
