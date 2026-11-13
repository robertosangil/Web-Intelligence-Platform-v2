using Wip.Persistence;

namespace Wip.Worker;

public sealed class CollectionSchedulerOptions
{
    public bool Enabled { get; set; }
    public int MaxConcurrency { get; set; } = 4;
    public int PollIntervalSeconds { get; set; } = 60;
    public List<CollectionDefinition> Definitions { get; set; } =
    [
        new("Cloudflare", "daily", TimeSpan.FromHours(24), true),
        new("GoDaddy", "daily", TimeSpan.FromHours(24), true),
        new("cPanel", "daily", TimeSpan.FromHours(24), true),
        new("WpUmbrella", "daily", TimeSpan.FromHours(24), true)
    ];
}

public sealed record CollectionDefinition(string Provider, string CollectorKey, TimeSpan Interval, bool Enabled);

public static class CollectionSchedulePlanner
{
    public static IReadOnlyList<CollectionDefinition> GetDue(
        IEnumerable<CollectionDefinition> definitions,
        DateTimeOffset now,
        IReadOnlyDictionary<string, DateTimeOffset> lastRuns)
        => definitions.Where(definition =>
                definition.Enabled &&
                (!lastRuns.TryGetValue(Key(definition), out var lastRun) || now - lastRun >= definition.Interval))
            .ToArray();

    public static string Key(CollectionDefinition definition) => $"{definition.Provider}:{definition.CollectorKey}";
}

public static class CollectionRetryPolicy
{
    public static bool IsRetryable(int? statusCode)
        => statusCode is null or 408 or 425 or 429 or >= 500;
}

public interface ICollectionRunOrchestrator
{
    Task<Guid> CreateRunAsync(CollectionDefinition definition, CancellationToken cancellationToken);
}

public sealed class CollectionRunOrchestrator(
    IServiceScopeFactory scopeFactory,
    ILogger<CollectionRunOrchestrator> logger) : ICollectionRunOrchestrator
{
    public async Task<Guid> CreateRunAsync(CollectionDefinition definition, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var run = new CollectionRun
        {
            Id = Guid.NewGuid(),
            Provider = definition.Provider,
            CollectorKey = definition.CollectorKey,
            Status = "Planned",
            CompletenessStatus = "NotStarted",
            StartedAtUtc = now,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
        var sourceAttempt = new CollectionSourceAttempt
        {
            Id = Guid.NewGuid(),
            CollectionRunId = run.Id,
            SourceKey = definition.CollectorKey,
            AttemptNumber = 1,
            Status = "NotStarted",
            IsRetryable = false,
            StartedAtUtc = now
        };

        await using var scope = scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<WipDbContext>();
        db.CollectionRuns.Add(run);
        db.CollectionSourceAttempts.Add(sourceAttempt);
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Created collection run {CollectionRunId} for {Provider}/{CollectorKey}.", run.Id, definition.Provider, definition.CollectorKey);
        return run.Id;
    }
}
