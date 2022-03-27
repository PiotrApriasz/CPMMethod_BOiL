namespace CPMMethod.Logic
{
    public class CPMLogic
    {
        private static uint GetMinEarlyFinish(Activity[] activities)
        {
            return 0;
        }
        private static uint GetMaxLateStart(Activity[] activities)
        {
            return 0;
        }
        private static Activity[] CalculateEarly(Activity[] activities)
        {
            foreach(Activity activity in activities)
            {
                activity.EarlyStart = GetMinEarlyFinish(activity.Preccessors);
                activity.EarlyFinish = activity.EarlyStart + activity.Duration;
            }

            return activities;
        }
        private static Activity[] CalculateLate(Activity[] activities)
        {
            foreach(Activity activity in activities)
            {
                activity.LateFinish = GetMaxLateStart(activity.Successors);
                activity.LateStart = activity.LateFinish - activity.Duration;
            }

            return activities;
        }
        private static String GetCriticalPath(Activity[] acitvities)
        {
            return String.Empty;
        }
    }
}