namespace Wip.Persistence;

public sealed class IngestionIdempotencyRecord
{
    public Guid Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Dataset { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public string PayloadHash { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime FirstSeenAtUtc { get; set; }
    public DateTime LastSeenAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
}
