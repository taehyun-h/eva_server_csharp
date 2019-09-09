using SimpleHttpServer.Models;
using System.Net;

namespace SimpleHttpServer
{
    public static class HttpDefaultBuilder
    {
        public static HttpResponse NotFound()
        {
            return new HttpResponse()
            {
                StatusCode = HttpStatusCode.NotFound,
                Reason = "NotFound",
                ContentUTF8 = "404: Not Found"
            };
        }

        public static HttpResponse MethodNotAllowed()
        {
            return new HttpResponse()
            {
                StatusCode = HttpStatusCode.MethodNotAllowed,
                Reason = "Method Not Allowed",
            };
        }

        public static HttpResponse InternalServerError()
        {
            return new HttpResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Reason = "InternalServerError",
                ContentUTF8 = "500: Internal Server Error"
            };
        }
    }
}
