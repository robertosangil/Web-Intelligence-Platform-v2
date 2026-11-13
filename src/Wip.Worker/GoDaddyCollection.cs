using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Worker;

public sealed record GoDaddyNormalizedDomain(
    string DomainKey,
    string Domain,
    string? Status,
    DateTime? ExpiresAtUtc,
    bool? AutoRenew,
    string PayloadJson);

public static class GoDaddyPayloadNormalizer
{
    public static GoDaddyNormalizedDomain NormalizeDomain(string domain, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        var root = document.RootElement;
        DateTime? expires = null;
        if (root.TryGetProperty("expiresAt", out var expiresValue) &&
            DateTime.TryParse(expiresValue.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parsed))
            expires = parsed;
        return new GoDaddyNormalizedDomain(
            domain.ToLowerInvariant(), domain, root.TryGetString("status"), expires,
            root.TryGetBoolean("autoRenew"), payloadJson);
    }

    public static string ProviderKey(string dataset, string value)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"godaddy|{dataset}|{value.ToLowerInvariant()}"))).ToLowerInvariant();

    private static string? TryGetString(this JsonElement root, string name)
        => root.TryGetProperty(name, out var value) ? value.GetString() : null;

    private static bool? TryGetBoolean(this JsonElement root, string name)
        => root.TryGetProperty(name, out var value) && value.ValueKind is JsonValueKind.True or JsonValueKind.False ? value.GetBoolean() : null;
}

public sealed class GoDaddyCollector
{
    public static bool IsRetryableStatus(int? statusCode) => CollectionRetryPolicy.IsRetryable(statusCode);

    public async Task<(int StatusCode, string PayloadJson)> FetchAsync(
        HttpClient client, Uri endpoint, string? apiKey, string? apiSecret, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiSecret))
            request.Headers.TryAddWithoutValidation("Authorization", $"sso-key {apiKey}:{apiSecret}");
        using var response = await client.SendAsync(request, cancellationToken);
        return ((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
    }
}

public sealed class GoDaddySqlIngestor(WipDbContext db)
{
    public async Task<bool> IngestDomainAsync(
        Guid collectionRunId, Guid sourceAttemptId, GoDaddyNormalizedDomain domain, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(domain.PayloadJson))).ToLowerInvariant();
        var key = GoDaddyPayloadNormalizer.ProviderKey("domains", domain.DomainKey);
        var existing = await db.IngestionIdempotency.FirstOrDefaultAsync(x =>
            x.Provider == "GoDaddy" && x.Dataset == "domains" && x.IdempotencyKey == key, cancellationToken);
        if (existing is not null)
        {
            existing.LastSeenAtUtc = now;
            existing.PayloadHash = hash;
            return false;
        }

        db.IngestionIdempotency.Add(new IngestionIdempotencyRecord
        {
            Id = Guid.NewGuid(), Provider = "GoDaddy", Dataset = "domains", IdempotencyKey = key,
            PayloadHash = hash, Status = "Processed", FirstSeenAtUtc = now, LastSeenAtUtc = now, ProcessedAtUtc = now
        });
        db.RawProviderPayloads.Add(new RawProviderPayload
        {
            Id = Guid.NewGuid(), CollectionRunId = collectionRunId, SourceAttemptId = sourceAttemptId,
            Provider = "GoDaddy", Dataset = "domains", PayloadHash = hash, PayloadJson = domain.PayloadJson, ReceivedAtUtc = now
        });
        db.GoDaddyDomainSnapshots.Add(new GoDaddyDomainSnapshot
        {
            Id = Guid.NewGuid(), DomainKey = domain.DomainKey, Domain = domain.Domain, Status = domain.Status,
            ExpiresAtUtc = domain.ExpiresAtUtc, AutoRenew = domain.AutoRenew, PayloadJson = domain.PayloadJson,
            CollectedAtUtc = now
        });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
