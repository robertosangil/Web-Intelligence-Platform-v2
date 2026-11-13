using System.Collections.Concurrent;

namespace Wip.Api;

internal sealed class PasswordResetRateLimiter
{
    private readonly ConcurrentDictionary<string, Window> windows = new(StringComparer.Ordinal);

    public bool IsAllowed(string email, string ipAddress, int limitPerEmail, int limitPerIp, TimeSpan window, DateTimeOffset now)
    {
        var emailAllowed = Consume($"email:{email}", limitPerEmail, window, now);
        var ipAllowed = Consume($"ip:{ipAddress}", limitPerIp, window, now);
        return emailAllowed && ipAllowed;
    }

    private bool Consume(string key, int limit, TimeSpan window, DateTimeOffset now)
    {
        if (limit <= 0) return false;
        var current = windows.AddOrUpdate(key,
            _ => new Window(now, 1),
            (_, existing) => existing.StartedAt + window <= now
                ? new Window(now, 1)
                : existing with { Count = existing.Count + 1 });

        return current.Count <= limit;
    }

    private sealed record Window(DateTimeOffset StartedAt, int Count);
}
