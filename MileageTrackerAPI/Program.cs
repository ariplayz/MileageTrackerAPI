using Microsoft.EntityFrameworkCore;
using MileageTrackerAPI.Models;

namespace MileageTrackerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<MileageDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(10, 5))
            )
        );
        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(80); // Replace 5219 with your desired port if needed
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapPost("/sync/create", async (MileageDbContext db) =>
        {
            var code = Guid.NewGuid().ToString().Substring(0, 6);
            var session = new SyncSession { LinkingCode = code, CreatedAt = DateTime.UtcNow };
            db.SyncSessions.Add(session);
            await db.SaveChangesAsync();
            return Results.Ok(new { session.Id, session.LinkingCode });
        });
        
        app.MapGet("/logs/all", async (MileageDbContext db) =>
        {
            var logs = await db.MileageLogs.ToListAsync();
            return Results.Ok(logs);
        });

        app.MapPost("/sync/join", async (string code, MileageDbContext db) =>
        {
            var session = await db.SyncSessions.FirstOrDefaultAsync(s => s.LinkingCode == code);
            if (session == null) return Results.NotFound();
            return Results.Ok(session);
        });

        app.MapPost("/logs/sync", async (MileageLog log, MileageDbContext db) =>
        {
            db.MileageLogs.Add(log);
            await db.SaveChangesAsync();
            return Results.Ok(log);
        });

        app.Run();
    }
}