namespace CPMMethod.Logic;

public class GanttActivity
{
    public string TaskId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double Duration { get; set; }
    public string Predecessor { get; set; }
    public string Description { get; set; }
}