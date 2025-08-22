using Microsoft.EntityFrameworkCore;
using MileageTrackerAPI.Models;

namespace MileageTrackerAPI;

public class MileageDbContext : DbContext
{
    public MileageDbContext(DbContextOptions<MileageDbContext> options) : base(options) { }
    public DbSet<SyncSession> SyncSessions { get; set; }
    public DbSet<MileageLog> MileageLogs { get; set; }
}