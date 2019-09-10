namespace eva_server
{
    public class StaticData : Singleton<StaticData>
    {
        public const int NewStudyWordCount = 30;
        public static readonly int[] StudyPeriodDate = {0, 2, 3, 5};
        public static readonly int[] TestPeriodDate = {5, 10, 20, 15, 30, 30, 30};

        public bool ShouldStudyNewWord(int date)
        {
            return date % 3 == 1 || date % 3 == 2;
        }
    }
}
