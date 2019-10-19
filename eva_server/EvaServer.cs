using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SimpleHttpServer;
using SimpleHttpServer.Models;

namespace eva_server
{
    public static class EvaServer
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
                new Route
                {
                    Callable = SignInResponse.Response,
                    UrlRegex = "^\\/sign_in",
                    Method = "GET"
                },
                new Route
                {
                    Callable = StudyWordResponse.Response,
                    UrlRegex = "^\\/study_word",
                    Method = "GET"
                },
                new Route
                {
                    Callable = TestWordIKnowResponse.Response,
                    UrlRegex = "^\\/test_word_i_know",
                    Method = "GET"
                },
                new Route
                {
                    Callable = TestWordIDontKnowResponse.Response,
                    UrlRegex = "^\\/test_word_i_dont_know",
                    Method = "GET"
                },
            };

        public static void Main(string[] args)
        {
            Console.WriteLine("Run Server");

            UpdateWordDataCount();

            var httpServer = new HttpServer(80, Get);
            var thread = new Thread(httpServer.Listen);
            thread.Start();
        }

        #region Static Data

        public const string WordDataPath = "Data/Static/WordData.txt";

        public static int WordDataCount { get; private set; }

        private static void UpdateWordDataCount()
        {
            WordDataCount = File.ReadLines(WordDataPath).Count();
        }

        #endregion

        #region Server Data

        private const string ProtocolUserPath = "Data/User/ProtocolUser.json";

        private static ProtocolUser _protocolUser;

        public static ProtocolUser GetProtocolUser()
        {
            if (_protocolUser == null)
            {
                var text = File.ReadAllText(ProtocolUserPath);
                _protocolUser = JsonUtil.DeserializeObject<ProtocolUser>(text);
            }

            return _protocolUser;
        }

        public static string GetProtocolUserString()
        {
            return JsonUtil.SerializeObjectWithIndentation(_protocolUser);
        }

        public static void SaveProtocolUser()
        {
            var text = GetProtocolUserString();
            FileUtil.WriteText(ProtocolUserPath, text);
        }

        #endregion
    }
}
