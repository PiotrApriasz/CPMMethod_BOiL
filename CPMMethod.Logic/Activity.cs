namespace CPMMethod.Logic
{
    public class Activity
    {
        public string Id { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public double Duration { get; set; }
        public double EarlyStart;
        public double LateStart;
        public double EarlyFinish;
        public double LateFinish;
        public Activity[] Successors { get; set; } = {};
        public Activity[] Preccessors { get; set; } = {};
    }
}