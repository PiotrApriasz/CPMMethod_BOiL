using System.Reflection;

namespace CPMMethod.Logic
{
    public static class CPMLogic
    {
        public static IEnumerable<Activity> CalculateEarly(this IEnumerable<Activity> activities)
        {
            foreach (Activity activity in activities)
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

        private static Activity[] Sort(Activity[] activities, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - 1) - 2;

                Sort(activities, l, m);
                Sort(activities, m + 1, r);
                Merge(activities, l, m, r);
            }
            
            return activities;
        }

        private static void Merge(Activity[] activities, int l, int m, int r)
        {
            int n1 = m - l + 1;
            int n2 = r - m;

            Activity[] L = new Activity[n1];
            Activity[] R = new Activity[n2];

            int i, j;
            for (i = 0; i < n1; i++)
                L[i] = activities[l + i];
            for (j = 0; j < n2; j++)
                R[j] = activities[m + 1 + j];

            int k = 1;
            i = 0;
            j = 0;
            while (i < n1 && j < n2)
            {
                foreach (Activity act in R[j].Preccessors)
                {
                    if (Convert.ToInt16(L[i].Id) <= Convert.ToInt16(act.Id))
                    {
                        activities[k] = L[i];
                        i++;
                    }
                    else
                    {
                        activities[k] = R[j];
                        j++;
                    }

                    k++;
                }
            }

            while (i < n1)
            {
                activities[k] = L[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                activities[k] = R[j];
                j++;
                k++;
            }
        }
    }
}