using System;
using System.Net;
using SimpleHttpServer.Models;

namespace eva_server
{
    public static class SignInResponse
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
            UpdateSignIn(protocolUser);
            EvaServer.SaveProtocolUser();
        }

        private static void UpdateSignIn(ProtocolUser protocolUser)
        {
            var now = DateTime.Now;
            var isFirstLoginToday = now.Year != protocolUser.LastSignInTime.Year || now.DayOfYear != protocolUser.LastSignInTime.DayOfYear;
            protocolUser.LastSignInTime = now;

            if (isFirstLoginToday)
            {
                RemoveLastTodayStudyWords(protocolUser);
                UpdateTodayWords(protocolUser);
            }
        }

        private static void RemoveLastTodayStudyWords(ProtocolUser protocolUser)
        {
            protocolUser.TodayStudyWords.Remove(protocolUser.TodayStudyDate);
        }

        private static void UpdateTodayWords(ProtocolUser protocolUser)
        {
            protocolUser.TodayStudyDate++;
            protocolUser.TodayStudyWordsIndex = 0;
            if (StaticData.Instance.ShouldStudyNewWord(protocolUser.TodayStudyDate))
            {
                AddNewStudyWords(protocolUser);
            }

            UpdateTodayWordStudyCount(protocolUser);
        }

        private static void AddNewStudyWords(ProtocolUser protocolUser)
        {
            for (var i = 0; i < StaticData.NewStudyWordCount; i++)
            {
                var id = protocolUser.LastStudiedWordId + 1;
                if (id >= EvaServer.WordDataCount) break;

                protocolUser.LastStudiedWordId++;
                protocolUser.WordStudyData[id] = new ProtocolUserWordStudyData
                {
                    Id = id,
                    StudyCount = 0,
                };
                protocolUser.AddStudyWord(protocolUser.TodayStudyDate, id);
            }
        }

        private static void UpdateTodayWordStudyCount(ProtocolUser protocolUser)
        {
            if (!protocolUser.TodayStudyWords.TryGetValue(protocolUser.TodayStudyDate, out var todayStudyWords)) return;

            foreach (var id in todayStudyWords)
            {
                var data = protocolUser.WordStudyData[id];
                data.StudyCount++;

                if (data.StudyCount >= StaticData.StudyPeriodDate.Length)
                {
                    AddTestWordToNextPeriod(protocolUser, data);
                }
                else
                {
                    AddStudyWordToNextPeriod(protocolUser, data);
                }
            }
        }

        private static void AddTestWordToNextPeriod(ProtocolUser protocolUser, ProtocolUserWordStudyData data)
        {
            var testData = new ProtocolUserWordTestData
            {
                Id = data.Id,
                LastPassedDate = protocolUser.TodayStudyDate,
                TestPassCount = 0
            };
            protocolUser.WordTestData[testData.Id] = testData;

            var nextStudyDate = protocolUser.TodayStudyDate + StaticData.TestPeriodDate[testData.TestPassCount];
            protocolUser.AddTestWord(nextStudyDate, testData.Id);
        }

        private static void AddStudyWordToNextPeriod(ProtocolUser protocolUser, ProtocolUserWordStudyData data)
        {
            var nextStudyDate = protocolUser.TodayStudyDate + StaticData.StudyPeriodDate[data.StudyCount];
            protocolUser.AddStudyWord(nextStudyDate, data.Id);
        }
    }
}
