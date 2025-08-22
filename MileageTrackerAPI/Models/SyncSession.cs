namespace MileageTrackerAPI.Models;

public class SyncSession
{
    public int Id { get; set; }
    public string LinkingCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<MileageLog> MileageLogs { get; set; }
}