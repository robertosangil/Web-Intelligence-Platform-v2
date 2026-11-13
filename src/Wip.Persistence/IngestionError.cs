namespace Wip.Persistence;

public sealed class IngestionError
{
    public Guid Id { get; set; }
    public Guid CollectionRunId { get; set; }
    public Guid? SourceAttemptId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRetryable { get; set; }
    public DateTime OccurredAtUtc { get; set; }
    public DateTime? NextAttemptAtUtc { get; set; }
    public DateTime? ResolvedAtUtc { get; set; }
}
