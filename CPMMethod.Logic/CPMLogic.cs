namespace CPMMethod.Logic
{
    public static class CPMLogic
    {
        public static IEnumerable<Activity> CalculateEarly(this IEnumerable<Activity> activities)
        {
            foreach(Activity activity in activities)
            {
                activity.EarlyStart =  activity.Preccessors?.Count > 0 ? activity.Preccessors.Min(actv => actv.EarlyFinish) : 0;
                activity.EarlyFinish = activity.EarlyStart + activity.Duration;
            }

            return activities;
        }
        public static IEnumerable<Activity> CalculateLate(this IEnumerable<Activity> activities)
        {
            var activitiesReversed = activities.Reverse();
            
            foreach(Activity activity in activitiesReversed)
            {
                activity.LateFinish = activity.Successors?.Count > 0 ? activity.Successors.Max(actv => actv.LateStart) : activity.EarlyFinish;
                activity.LateStart = activity.LateFinish - activity.Duration;
            }

            return activitiesReversed.Reverse();
        }
        public static IEnumerable<Activity> GetCriticalPath(this IEnumerable<Activity> acitvities)
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