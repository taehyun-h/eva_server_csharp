using System;
using System.Collections.Generic;

namespace eva_server
{
    [Serializable]
    public class ProtocolUser
    {
        public DateTime LastSignInTime;
        public int TodayStudyDate;
        public int LastStudiedWordId;
        public Dictionary<int, ProtocolUserWordStudyData> WordStudyData = new Dictionary<int, ProtocolUserWordStudyData>();
        public Dictionary<int, ProtocolUserWordTestData> WordTestData = new Dictionary<int, ProtocolUserWordTestData>();

        public int TodayStudyWordsIndex;
        public Dictionary<int, List<int>> TodayStudyWords = new Dictionary<int, List<int>>();

        public int TodayTestWordsIndex;
        public Dictionary<int, List<int>> TodayTestWords = new Dictionary<int, List<int>>();

        public void AddStudyWord(int date, int id)
        {
            if (!TodayStudyWords.TryGetValue(date, out var list))
            {
                list = new List<int>();
                TodayStudyWords[date] = list;
            }

            list.Add(id);
        }

        public void AddTestWord(int date, int id)
        {
            if (!TodayTestWords.TryGetValue(date, out var list))
            {
                list = new List<int>();
                TodayTestWords[date] = list;
            }

            list.Add(id);
        }
    }
}
