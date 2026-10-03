using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Models;

namespace MusicPlatform.Api.Data;
public sealed class MusicDbContext(DbContextOptions<MusicDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
}

public static class SeedData
{
    public static void Initialize(MusicDbContext db)
    {
        if (db.Users.Any()) return;
        db.Users.Add(new User { Name = "Demo User", Email = "demo@music.local" });
        db.Plans.AddRange(
            new Plan { Name = "Free", Price = 0, Description = "Acceso con anuncios" },
            new Plan { Name = "Premium", Price = 9.99m, Description = "Música sin anuncios y offline" },
            new Plan { Name = "Family", Price = 14.99m, Description = "Hasta seis cuentas" });
        db.Songs.AddRange(
            new Song { Title = "Midnight Drive", Artist = "Neon Waves", Album = "After Hours", Genre = "Electronic", DurationSeconds = 214, CoverUrl = "https://picsum.photos/seed/neon/500/500" },
            new Song { Title = "Summer Rain", Artist = "Luna García", Album = "Horizonte", Genre = "Pop", DurationSeconds = 188, CoverUrl = "https://picsum.photos/seed/luna/500/500" },
            new Song { Title = "Open Roads", Artist = "The Travelers", Album = "Miles Away", Genre = "Rock", DurationSeconds = 245, CoverUrl = "https://picsum.photos/seed/roads/500/500" },
            new Song { Title = "Blue Notes", Artist = "Milo Jazz", Album = "Late Session", Genre = "Jazz", DurationSeconds = 302, CoverUrl = "https://picsum.photos/seed/jazz/500/500" });
        db.SaveChanges();
    }
}
