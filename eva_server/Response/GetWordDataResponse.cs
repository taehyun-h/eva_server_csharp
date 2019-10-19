using System.IO;
using System.Net;
using SimpleHttpServer.Models;

namespace eva_server
{
    public static class GetWordDataResponse
    {
        public static HttpResponse Response(HttpRequest request)
        {
            return new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                Reason = "OK",
                ContentUTF8 = File.ReadAllText(EvaServer.WordDataPath),
            };
        }
    }
}
