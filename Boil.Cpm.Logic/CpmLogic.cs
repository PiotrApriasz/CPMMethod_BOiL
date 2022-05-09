
// ReSharper disable PossibleMultipleEnumeration

using System.Runtime.Intrinsics.X86;
using Boil.Cpm.Logic;

namespace Boil.Cpm.Logic
{
    public static class CpmLogic
    {
        private static void MoveForward(this IEnumerable<Activity> activities)
        {
            foreach (var activity in activities)
            {
                activity.EarlyStart =  activity.Preccessors.Count > 0 ? activity.Preccessors.Max(actv => actv.EarlyFinish) : 0;
                activity.EarlyFinish = activity.EarlyStart + activity.Duration;
                activity.Successors = new List<Activity>();

                foreach(var preccessor in activity.Preccessors)
                {
                    preccessor.Successors.Add(activity);
                }
            }
        }

        private static void MoveBackward(this IEnumerable<Activity> activities)
        {
            var fullTime = activities.FindFullTime();
            foreach(var activity in activities.Reverse())
            {
                activity.LateFinish =
                    activity.Successors.Count > 0 ? activity.Successors.Min(actv => actv.LateStart) : fullTime;
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
            
            // Calculate reserve time
            activities.ForEach(actv => actv.Reserve = actv.LateStart - actv.EarlyStart);

            // Find all activities witch belong to critical paths
            activities.ForEach(actv => actv.Critical = Math.Abs(actv.EarlyStart - actv.LateStart)
                < 1E-20 && Math.Abs(actv.EarlyFinish - actv.LateFinish) < 1E-20);
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
                    activity.Preccessors.Add(activities.Find(x => x.Id.ToString() == preActivity)
                                             ?? throw new InvalidOperationException());
                }
            }
        }

        public static List<GanttActivity> CalculateGanttActivities(this IEnumerable<Activity> activities)
        {
            return (from activity in activities
                let duration = (int)(activity.LateFinish - activity.EarlyStart)
                select new GanttActivity()
                {
                    TaskId = activity.Id, 
                    TaskName = activity.Name,
                    Duration = duration.ToString(),
                    Predecessor = activity.PreActivities,
                }).ToList();
        }
        
        public static bool CheckPredecessorsCorrectness(this IEnumerable<Activity> activities, string newPreActivities)
        {
            var result = false;
            
            var preActivities = newPreActivities.Split(',');

            var predecessorsCorectness = preActivities.ToDictionary(preActivity => preActivity, _ => false);

            foreach (var activity in activities)
            {
                if (activity.PreActivities == "-") continue;
                
                if (predecessorsCorectness.ContainsKey(activity.Id.ToString()))
                    predecessorsCorectness[activity.Id.ToString()] = true;

                if (predecessorsCorectness.Values.All(x => x.Equals(true)))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static List<GanttActivity> CalculateAsapGanttActivities(this IEnumerable<Activity> activities)
        {
            var ganttActivities = new List<GanttActivity>()
            {
                new()
                {
                    TaskId = 1,
                    TaskName = "A",
                    Duration = "5",
                    Predecessor = "-",
                    Progress = 100
                },
                new()
                {
                    TaskId = 2,
                    TaskName = "B",
                    Duration = "7",
                    Predecessor = "-",
                    Progress = 100
                },
                new()
                {
                    TaskId = 3,
                    TaskName = "C",
                    Duration = "6",
                    Predecessor = "1",
                    Progress = 100
                },
                new()
                {
                    TaskId = 4,
                    TaskName = "D",
                    Duration = "10",
                    Predecessor = "1",
                    //Progress = ((10 - 2) / 10) * 100 //(Duration - Reserve) / Duration * 100
                    Progress = 80 //wychodzi na to ze ten procent musi byc policzony wczesniej bo tak jak wyzej to nie pokaze sie na wykresie
                },
                new()
                {
                    TaskId = 5,
                    TaskName = "E",
                    Duration = "8",
                    Predecessor = "2",
                    Progress = 37.5
                },
                new()
                {
                    TaskId = 6,
                    TaskName = "F",
                    Duration = "4",
                    Predecessor = "3",
                    Progress = 100
                },
                new()
                {
                    TaskId = 7,
                    TaskName = "G",
                    Duration = "9",
                    Predecessor = "3",
                    Progress = 22.22
                },
                new()
                {
                    TaskId = 8,
                    TaskName = "H",
                    Duration = "5",
                    Predecessor = "4,5,6",
                    Progress = 100
                },
            };

            return ganttActivities;
        }

        private static double FindFullTime(this IEnumerable<Activity> activities)
        {
            return activities.Select(activity => activity.EarlyFinish).Prepend(0).Max();
        }
    }
}