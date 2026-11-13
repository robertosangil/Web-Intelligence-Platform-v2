namespace Wip.Persistence;

public sealed class CollectionRun
{
    public Guid Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string CollectorKey { get; set; } = string.Empty;
    public DateTime? RequestedFromUtc { get; set; }
    public DateTime? RequestedToUtc { get; set; }
    public string? WorkflowId { get; set; }
    public string? ExecutionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CompletenessStatus { get; set; }
    public int RetryCount { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
