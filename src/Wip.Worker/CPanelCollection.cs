using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Worker;

public sealed record CPanelNormalizedResource(
    string ResourceKey,
    string Account,
    string ResourceName,
    decimal? CurrentValue,
    decimal? LimitValue,
    string? Unit,
    string? Status,
    string PayloadJson);

public static class CPanelPayloadNormalizer
{
    public static CPanelNormalizedResource NormalizeResource(string account, string resourceName, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        var root = document.RootElement;
        return new CPanelNormalizedResource(
            $"{account}:{resourceName}", account, resourceName,
            ReadDecimal(root, "current", "usage"), ReadDecimal(root, "limit"),
            TryGetString(root, "unit"), TryGetString(root, "status"), payloadJson);
    }

    public static string ProviderKey(string dataset, string account, string value)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"cpanel|{dataset}|{account}|{value}"))).ToLowerInvariant();

    private static decimal? ReadDecimal(JsonElement root, params string[] names)
        => names.Select(name => root.TryGetProperty(name, out var value) && value.TryGetDecimal(out var number) ? number : (decimal?)null)
            .FirstOrDefault(value => value.HasValue);

    private static string? TryGetString(JsonElement root, string name)
        => root.TryGetProperty(name, out var value) ? value.GetString() : null;
}

public sealed class CPanelCollector
{
    public static bool IsRetryableStatus(int? statusCode) => CollectionRetryPolicy.IsRetryable(statusCode);

    public async Task<(int StatusCode, string PayloadJson)> FetchAsync(
        HttpClient client, Uri endpoint, string? account, string? apiToken, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrWhiteSpace(account) && !string.IsNullOrWhiteSpace(apiToken))
            request.Headers.TryAddWithoutValidation("Authorization", $"cpanel {account}:{apiToken}");
        using var response = await client.SendAsync(request, cancellationToken);
        return ((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
    }
}

public sealed class CPanelSqlIngestor(WipDbContext db)
{
    public async Task<bool> IngestResourceAsync(
        Guid collectionRunId, Guid sourceAttemptId, CPanelNormalizedResource resource, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(resource.PayloadJson))).ToLowerInvariant();
        var key = CPanelPayloadNormalizer.ProviderKey("resources", resource.Account, resource.ResourceName);
        var existing = await db.IngestionIdempotency.FirstOrDefaultAsync(x =>
            x.Provider == "cPanel" && x.Dataset == "resources" && x.IdempotencyKey == key, cancellationToken);
        if (existing is not null)
        {
            existing.LastSeenAtUtc = now;
            existing.PayloadHash = hash;
            return false;
        }

        db.IngestionIdempotency.Add(new IngestionIdempotencyRecord
        {
            Id = Guid.NewGuid(), Provider = "cPanel", Dataset = "resources", IdempotencyKey = key,
            PayloadHash = hash, Status = "Processed", FirstSeenAtUtc = now, LastSeenAtUtc = now, ProcessedAtUtc = now
        });
        db.RawProviderPayloads.Add(new RawProviderPayload
        {
            Id = Guid.NewGuid(), CollectionRunId = collectionRunId, SourceAttemptId = sourceAttemptId,
            Provider = "cPanel", Dataset = "resources", PayloadHash = hash, PayloadJson = resource.PayloadJson, ReceivedAtUtc = now
        });
        db.CPanelResources.Add(new CPanelResource
        {
            Id = Guid.NewGuid(), ResourceKey = resource.ResourceKey, Account = resource.Account,
            ResourceName = resource.ResourceName, CurrentValue = resource.CurrentValue, LimitValue = resource.LimitValue,
            Unit = resource.Unit, Status = resource.Status, PayloadJson = resource.PayloadJson, CollectedAtUtc = now
        });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
