using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Worker;

public sealed record WpUmbrellaNormalizedProject(
    string ProjectKey,
    string SiteId,
    string SiteName,
    string ProjectId,
    string? ProjectName,
    string? Status,
    string PayloadJson);

public static class WpUmbrellaPayloadNormalizer
{
    public static WpUmbrellaNormalizedProject NormalizeProject(string siteId, string siteName, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        var root = document.RootElement;
        var projectId = TryGetString(root, "projectId") ?? "unknown";
        return new WpUmbrellaNormalizedProject(
            $"{siteId}:{projectId}", siteId, siteName, projectId,
            TryGetString(root, "projectName"), TryGetString(root, "status"), payloadJson);
    }

    public static string ProviderKey(string dataset, string siteId, string value)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"wp-umbrella|{dataset}|{siteId}|{value}"))).ToLowerInvariant();

    private static string? TryGetString(JsonElement root, string name)
        => root.TryGetProperty(name, out var value) ? value.GetString() : null;
}

public sealed class WpUmbrellaCollector
{
    public static bool IsRetryableStatus(int? statusCode) => CollectionRetryPolicy.IsRetryable(statusCode);

    public async Task<(int StatusCode, string PayloadJson)> FetchAsync(
        HttpClient client, Uri endpoint, string? apiToken, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrWhiteSpace(apiToken))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
        using var response = await client.SendAsync(request, cancellationToken);
        return ((int)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
    }
}

public sealed class WpUmbrellaSqlIngestor(WipDbContext db)
{
    public async Task<bool> IngestProjectAsync(
        Guid collectionRunId, Guid sourceAttemptId, WpUmbrellaNormalizedProject project, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(project.PayloadJson))).ToLowerInvariant();
        var key = WpUmbrellaPayloadNormalizer.ProviderKey("projects", project.SiteId, project.ProjectId);
        var existing = await db.IngestionIdempotency.FirstOrDefaultAsync(x =>
            x.Provider == "WP Umbrella" && x.Dataset == "projects" && x.IdempotencyKey == key, cancellationToken);
        if (existing is not null)
        {
            existing.LastSeenAtUtc = now;
            existing.PayloadHash = hash;
            return false;
        }

        db.IngestionIdempotency.Add(new IngestionIdempotencyRecord
        {
            Id = Guid.NewGuid(), Provider = "WP Umbrella", Dataset = "projects", IdempotencyKey = key,
            PayloadHash = hash, Status = "Processed", FirstSeenAtUtc = now, LastSeenAtUtc = now, ProcessedAtUtc = now
        });
        db.RawProviderPayloads.Add(new RawProviderPayload
        {
            Id = Guid.NewGuid(), CollectionRunId = collectionRunId, SourceAttemptId = sourceAttemptId,
            Provider = "WP Umbrella", Dataset = "projects", PayloadHash = hash, PayloadJson = project.PayloadJson, ReceivedAtUtc = now
        });
        db.WpUmbrellaProjectSnapshots.Add(new WpUmbrellaProjectSnapshot
        {
            Id = Guid.NewGuid(), ProjectKey = project.ProjectKey, SiteId = project.SiteId, SiteName = project.SiteName,
            ProjectId = project.ProjectId, ProjectName = project.ProjectName, Status = project.Status,
            PayloadJson = project.PayloadJson, CollectedAtUtc = now
        });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
