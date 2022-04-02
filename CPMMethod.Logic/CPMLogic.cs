using System.Diagnostics;
using System.Reflection;

namespace CPMMethod.Logic
{
    public static class CPMLogic
    {
        public static void MoveForward(this IEnumerable<Activity> activities)
        {
            foreach (var activity in activities)
            {
                activity.EarlyStart =  activity.Preccessors?.Count > 0 ? activity.Preccessors.Max(actv => actv.EarlyFinish) : 0;
                activity.EarlyFinish = activity.EarlyStart + activity.Duration;
                activity.Successors = new List<Activity>();

                foreach(var preccessor in activity.Preccessors ?? Enumerable.Empty<Activity>())
                {
                    preccessor.Successors.Add(activity);
                }
            }
        }
        public static void MoveBackward(this IEnumerable<Activity> activities)
        {
            foreach(var activity in activities.Reverse())
            {
                activity.LateFinish = activity.Successors?.Count > 0 ? activity.Successors.Min(actv => actv.LateStart) : activity.EarlyFinish;
                activity.LateStart = activity.LateFinish - activity.Duration;
            }
        }
        public static void InitActivitiesFields(this List<Activity> activities)
        {
            // Sort list of activites to ensure that all of activity preccessors are being calculated in advance
            activities.Sort((lactv, pactv) => lactv.Preccessors.Contains(pactv) ? 1 : -1);

            // Go back and forth threw acitivity list to fill all of the activity fields
            activities.MoveForward();
            activities.MoveBackward();

            // Find all activities witch belong to critical paths
            activities.ForEach(actv => actv.Critical = actv.EarlyStart == actv.LateStart && actv.EarlyFinish == actv.LateFinish);
        }

        public static void CalculatePreccessors(this List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                activity.Preccessors = new List<Activity>();

                if (activity.PreActivities == "-") continue;
                var preActivities = activity.PreActivities.Split(',');
                foreach (var preActivity in preActivities)
                {
                    activity.Preccessors.Add(activities.Find(x => x.Id == preActivity)
                                             ?? throw new InvalidOperationException());
                }
            }
        }
    }
}