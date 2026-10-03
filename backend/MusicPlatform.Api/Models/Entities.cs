namespace MusicPlatform.Api.Models;

public enum SubscriptionStatus { Active, Cancelled, Expired }
public sealed class User { public int Id { get; set; } public string Name { get; set; } = ""; public string Email { get; set; } = ""; }
public sealed class Plan { public int Id { get; set; } public string Name { get; set; } = ""; public decimal Price { get; set; } public string Description { get; set; } = ""; }
public sealed class Song { public int Id { get; set; } public string Title { get; set; } = ""; public string Artist { get; set; } = ""; public string Album { get; set; } = ""; public string Genre { get; set; } = ""; public int DurationSeconds { get; set; } public string CoverUrl { get; set; } = ""; }
public sealed class Subscription { public int Id { get; set; } public int UserId { get; set; } public int PlanId { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public SubscriptionStatus Status { get; set; } public Plan? Plan { get; set; } }
public sealed class Playlist { public int Id { get; set; } public int UserId { get; set; } public string Name { get; set; } = ""; public ICollection<Song> Songs { get; set; } = new List<Song>(); }
public sealed record CreateSubscriptionRequest(int UserId, int PlanId);
public sealed record CreatePlaylistRequest(int UserId, string Name);
public sealed record ReportRequest(int RequestedBy);
