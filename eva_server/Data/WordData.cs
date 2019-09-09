using System;

namespace eva_server
{
    [Serializable]
    public class WordData
    {
        public int Id;
        public string Spelling;
        public string[] Meanings;
    }
}
