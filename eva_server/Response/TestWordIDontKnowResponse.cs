using System.Net;
using SimpleHttpServer.Models;

namespace eva_server
{
    public static class TestWordIDontKnowResponse
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
            UpdateTestWord(protocolUser);
            UpdateTodayTestWord(protocolUser);
            EvaServer.SaveProtocolUser();
        }

        private static void UpdateTestWord(ProtocolUser protocolUser)
        {
            if (!protocolUser.TodayTestWords.TryGetValue(protocolUser.TodayStudyDate, out var todayTestWords)) return;

            var id = todayTestWords[protocolUser.TodayTestWordsIndex];
            if (!protocolUser.WordTestData.TryGetValue(id, out var data)) return;

            data.LastPassedDate = protocolUser.TodayStudyDate;

            var nextTestData = protocolUser.TodayStudyDate + StaticData.TestPeriodDate[data.TestPassCount];
            protocolUser.AddTestWord(nextTestData, data.Id);
        }

        private static void UpdateTodayTestWord(ProtocolUser protocolUser)
        {
            if (!protocolUser.TodayTestWords.TryGetValue(protocolUser.TodayStudyDate, out var todayTestWords)) return;

            protocolUser.TodayTestWordsIndex++;
            if (protocolUser.TodayTestWordsIndex >= todayTestWords.Count)
            {
                protocolUser.TodayTestWordsIndex = 0;
                protocolUser.TodayTestWords.Remove(protocolUser.TodayStudyDate);
            }
            else
            {
                protocolUser.TodayTestWordsIndex %= protocolUser.TodayStudyDate;
            }
        }
    }
}
