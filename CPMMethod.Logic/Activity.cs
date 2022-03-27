namespace CPMMethod.Logic
{
    public class Activity
    {
        public string Id { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public uint Duration { get; set; }
        public uint EarlyStart;
        public uint LateStart;
        public uint EarlyFinish;
        public uint LateFinish;
        public Activity[] Successors { get; set; } 
        public Activity[] Preccessors { get; set; }
    }
}