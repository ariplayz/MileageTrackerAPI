namespace MileageTrackerAPI.Models;

public class MileageLog
{
    public int Id { get; set; }
    public int SyncSessionId { get; set; }
    public DateTime Date { get; set; }
    public double Miles { get; set; }
    public string Description { get; set; }
    public SyncSession SyncSession { get; set; }
}