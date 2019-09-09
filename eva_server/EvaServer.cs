using System;
using System.Collections.Generic;
using System.Threading;
using SimpleHttpServer;
using SimpleHttpServer.Models;

namespace eva_server
{
    static class EvaServer
    {
        private static List<Route> Get =>
            new List<Route>
            {
                new Route
                {
                    Callable = GetWordDataResponse.Response,
                    UrlRegex = "^\\/get_word_data",
                    Method = "GET"
                },
            };

        static void Main(string[] args)
        {
            Console.WriteLine("Run Server");

            var httpServer = new HttpServer(80, Get);
            var thread = new Thread(httpServer.Listen);
            thread.Start();
        }
    }
}
