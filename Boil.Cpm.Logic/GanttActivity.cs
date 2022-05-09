namespace Boil.Cpm.Logic;

public class GanttActivity
{
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Duration { get; set; }
    public string Predecessor { get; set; }
    public double Progress { get; set; } = 100;
    public List<GanttActivity> SubTasks { get; set; }

}