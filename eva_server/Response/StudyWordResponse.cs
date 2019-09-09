using System.Net;
using SimpleHttpServer.Models;

namespace eva_server
{
    public static class StudyWordResponse
    {
        public static HttpResponse Response(HttpRequest request)
        {
            Process();
            return new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                Reason = "OK",
                ContentUTF8 = EvaServer.GetProtocolUserString(),
            };
        }

        private static void Process()
        {
            var protocolUser = EvaServer.GetProtocolUser();
            UpdateTodayStudyWordsIndex(protocolUser);
            EvaServer.SaveProtocolUser();
        }

        private static void UpdateTodayStudyWordsIndex(ProtocolUser protocolUser)
        {
            if (!protocolUser.TodayStudyWords.TryGetValue(protocolUser.TodayStudyDate, out var todayStudyWords)) return;

            protocolUser.TodayStudyWordsIndex = (protocolUser.TodayStudyWordsIndex + 1) % todayStudyWords.Count;
        }
    }
}
