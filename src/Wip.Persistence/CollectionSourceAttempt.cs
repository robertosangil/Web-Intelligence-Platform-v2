namespace Wip.Persistence;

public sealed class CollectionSourceAttempt
{
    public Guid Id { get; set; }
    public Guid CollectionRunId { get; set; }
    public string SourceKey { get; set; } = string.Empty;
    public string? Endpoint { get; set; }
    public int AttemptNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? HttpStatusCode { get; set; }
    public bool IsRetryable { get; set; }
    public string? ErrorCategory { get; set; }
    public string? ErrorMessage { get; set; }
    public string? PayloadHash { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? NextAttemptAtUtc { get; set; }
}
