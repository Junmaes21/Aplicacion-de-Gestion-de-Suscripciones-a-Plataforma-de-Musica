using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=music.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MusicDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}
app.UseCors();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "music-platform-api" }));
app.MapGet("/music", async (MusicDbContext db, string? search, string? genre) =>
{
    var query = db.Songs.AsNoTracking().AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Title.Contains(search) || x.Artist.Contains(search) || x.Album.Contains(search));
    if (!string.IsNullOrWhiteSpace(genre)) query = query.Where(x => x.Genre == genre);
    return Results.Ok(await query.OrderBy(x => x.Title).ToListAsync());
});
app.MapGet("/music/{id:int}", async (int id, MusicDbContext db) =>
    await db.Songs.FindAsync(id) is { } song ? Results.Ok(song) : Results.NotFound());

app.MapGet("/plans", async (MusicDbContext db) => Results.Ok(await db.Plans.AsNoTracking().ToListAsync()));
app.MapGet("/subscriptions/{userId:int}", async (int userId, MusicDbContext db) =>
{
    var subscription = await db.Subscriptions.AsNoTracking()
        .Include(x => x.Plan).Where(x => x.UserId == userId && x.Status == SubscriptionStatus.Active)
        .OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
    return subscription is null ? Results.NotFound(new { message = "No active subscription." }) : Results.Ok(subscription);
});
app.MapPost("/subscriptions", async (CreateSubscriptionRequest request, MusicDbContext db) =>
{
    if (request.UserId <= 0 || request.PlanId <= 0) return Results.BadRequest(new { message = "UserId and PlanId must be positive." });
    var plan = await db.Plans.FindAsync(request.PlanId);
    if (plan is null) return Results.NotFound(new { message = "Plan not found." });
    var user = await db.Users.FindAsync(request.UserId);
    if (user is null) return Results.NotFound(new { message = "User not found." });
    var now = DateTime.UtcNow;
    var subscription = new Subscription { UserId = user.Id, PlanId = plan.Id, StartDate = now, EndDate = now.AddMonths(1), Status = SubscriptionStatus.Active };
    db.Subscriptions.Add(subscription);
    await db.SaveChangesAsync();
    subscription.Plan = plan;
    return Results.Created($"/subscriptions/{subscription.Id}", subscription);
});
app.MapDelete("/subscriptions/{id:int}", async (int id, MusicDbContext db) =>
{
    var subscription = await db.Subscriptions.FindAsync(id);
    if (subscription is null) return Results.NotFound();
    subscription.Status = SubscriptionStatus.Cancelled;
    subscription.EndDate = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/playlists/{userId:int}", async (int userId, MusicDbContext db) =>
    Results.Ok(await db.Playlists.AsNoTracking().Include(x => x.Songs).Where(x => x.UserId == userId).ToListAsync()));
app.MapPost("/playlists", async (CreatePlaylistRequest request, MusicDbContext db) =>
{
    if (request.UserId <= 0 || string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest(new { message = "UserId and playlist name are required." });
    if (await db.Users.FindAsync(request.UserId) is null) return Results.NotFound(new { message = "User not found." });
    var playlist = new Playlist { UserId = request.UserId, Name = request.Name.Trim() };
    db.Playlists.Add(playlist);
    await db.SaveChangesAsync();
    return Results.Created($"/playlists/{playlist.Id}", playlist);
});
app.MapPost("/reports", async (ReportRequest request, MusicDbContext db) =>
{
    var users = await db.Users.CountAsync();
    var activeSubscriptions = await db.Subscriptions.CountAsync(x => x.Status == SubscriptionStatus.Active);
    var songs = await db.Songs.CountAsync();
    return Results.Ok(new { requestedBy = request.RequestedBy, generatedAt = DateTime.UtcNow, users, activeSubscriptions, songs });
});

app.Run();

public partial class Program { }
