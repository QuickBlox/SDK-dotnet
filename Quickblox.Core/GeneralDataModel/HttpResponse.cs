using Quickblox.Sdk.GeneralDataModel;
using System.Net;

namespace Quickblox.Sdk.Http
{
    public class HttpResponse
    {
        public Error Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class HttpResponse<TResult> : HttpResponse
    {
        public TResult Result { get; set; }
    }
}
