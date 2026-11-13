namespace Wip.Persistence;

public sealed class RawProviderPayload
{
    public Guid Id { get; set; }
    public Guid CollectionRunId { get; set; }
    public Guid? SourceAttemptId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Dataset { get; set; } = string.Empty;
    public string PayloadHash { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = string.Empty;
    public DateTime ReceivedAtUtc { get; set; }
    public DateTime? RetentionUntilUtc { get; set; }
}
