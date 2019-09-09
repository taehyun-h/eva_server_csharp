using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using SimpleHttpServer;
using SimpleHttpServer.Models;

namespace eva_server
{
    static class EvaServer
    {
        private static List<Route> Get =>
            new List<Route>()
            {
                new Route()
                {
                    Callable = Test,
                    UrlRegex = "^\\/$",
                    Method = "GET"
                },
            };

        private static HttpResponse Test(HttpRequest request)
        {
            return new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                Reason = "OK",
                ContentUTF8 = "Test",
            };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Run Server");

            var httpServer = new HttpServer(80, Get);
            var thread = new Thread(httpServer.Listen);
            thread.Start();
        }
    }
}
