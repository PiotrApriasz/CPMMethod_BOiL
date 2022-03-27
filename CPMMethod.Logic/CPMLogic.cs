namespace CPMMethod.Logic
{
    public class CPMLogic
    {
        private static Activity[] CalculateEarly(Activity[] activities)
        {
            foreach(Activity activity in activities)
            {
                activity.EarlyStart =  activity.Preccessors?.Length > 0 ? activity.Preccessors.Min(actv => actv.EarlyFinish) : 0;
                activity.EarlyFinish = activity.EarlyStart + activity.Duration;
            }

            return activities;
        }
        private static Activity[] CalculateLate(Activity[] activities)
        {
            foreach(Activity activity in activities)
            {
                activity.LateFinish = activity.Successors?.Length > 0 ? activity.Successors.Max(actv => actv.LateStart) : activity.EarlyFinish;
                activity.LateStart = activity.LateFinish - activity.Duration;
            }

            return activities;
        }
        public static Activity[] GetCriticalPath(Activity[] acitvities)
        {
            Activity[] CriticalPath = {};

            // Sort acitvities
            CalculateEarly(acitvities);
            CalculateLate(acitvities);

            foreach(Activity activity in acitvities)
            {
                if(activity.LateStart - activity.EarlyStart == 0 && activity.LateFinish - activity.LateStart == 0)
                {
                    CriticalPath.Append(activity);
                }
            }

            return CriticalPath;
        }
    }
}