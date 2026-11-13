using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Worker;

public sealed record CloudflareNormalizedDailyReport(
    string RecordKey,
    string ZoneId,
    string ZoneName,
    string ReportDateKey,
    long? TotalRequests,
    long? TotalBandwidthBytes,
    string PayloadJson);

public static class CloudflarePayloadNormalizer
{
    public static CloudflareNormalizedDailyReport Normalize(string zoneId, string zoneName, string reportDateKey, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        var root = document.RootElement;
        return new CloudflareNormalizedDailyReport(
            $"{zoneId}:{reportDateKey}", zoneId, zoneName, reportDateKey,
            ReadLong(root, "requests", "totalRequests"),
            ReadLong(root, "bandwidth", "totalBandwidthBytes"),
            payloadJson);
    }

    public static string IdempotencyKey(string runId, string dataset, string zoneId, string reportDateKey)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{runId}|{dataset}|{zoneId}|{reportDateKey}"))).ToLowerInvariant();

    private static long? ReadLong(JsonElement root, params string[] names)
        => names.Select(name => root.TryGetProperty(name, out var value) && value.TryGetInt64(out var number) ? number : (long?)null)
            .FirstOrDefault(value => value.HasValue);
}

public sealed class CloudflareCollector
{
    public static bool IsRetryableStatus(int? statusCode) => CollectionRetryPolicy.IsRetryable(statusCode);

    public async Task<(int StatusCode, string PayloadJson)> FetchAsync(HttpClient client, Uri endpoint, string? apiToken, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrWhiteSpace(apiToken))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);

        using var response = await client.SendAsync(request, cancellationToken);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        return ((int)response.StatusCode, payload);
    }
}

public sealed class CloudflareSqlIngestor(WipDbContext db)
{
    public async Task<bool> IngestDailyReportAsync(
        Guid collectionRunId,
        Guid sourceAttemptId,
        CloudflareNormalizedDailyReport report,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(report.PayloadJson))).ToLowerInvariant();
        var idempotencyKey = CloudflarePayloadNormalizer.IdempotencyKey(collectionRunId.ToString("N"), "daily", report.ZoneId, report.ReportDateKey);
        var existing = await db.IngestionIdempotency.FirstOrDefaultAsync(x =>
            x.Provider == "Cloudflare" && x.Dataset == "daily" && x.IdempotencyKey == idempotencyKey, cancellationToken);
        if (existing is not null)
        {
            existing.LastSeenAtUtc = now;
            existing.PayloadHash = hash;
            return false;
        }

        db.IngestionIdempotency.Add(new IngestionIdempotencyRecord
        {
            Id = Guid.NewGuid(), Provider = "Cloudflare", Dataset = "daily", IdempotencyKey = idempotencyKey,
            PayloadHash = hash, Status = "Processed", FirstSeenAtUtc = now, LastSeenAtUtc = now, ProcessedAtUtc = now
        });
        db.RawProviderPayloads.Add(new RawProviderPayload
        {
            Id = Guid.NewGuid(), CollectionRunId = collectionRunId, SourceAttemptId = sourceAttemptId,
            Provider = "Cloudflare", Dataset = "daily", PayloadHash = hash, PayloadJson = report.PayloadJson, ReceivedAtUtc = now
        });
        db.CloudflareDailyReports.Add(new CloudflareDailyReport
        {
            Id = Guid.NewGuid(), RecordKey = report.RecordKey, ZoneId = report.ZoneId, ZoneName = report.ZoneName,
            ReportDateKey = report.ReportDateKey, TotalRequests = report.TotalRequests,
            TotalBandwidthBytes = report.TotalBandwidthBytes, PayloadJson = report.PayloadJson, CollectedAtUtc = now,
            CollectionStatus = "Complete", DataQualityStatus = "Observed"
        });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
