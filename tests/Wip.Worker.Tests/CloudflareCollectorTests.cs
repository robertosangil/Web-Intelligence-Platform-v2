using Wip.Worker;

namespace Wip.Worker.Tests;

public sealed class CloudflareCollectorTests
{
    [Fact]
    public void NormalizerCreatesStableDatasetKeyAndPreservesRawPayload()
    {
        var result = CloudflarePayloadNormalizer.Normalize(
            "zone-1",
            "example.com",
            "2026-11-13",
            "{\"requests\":42,\"bandwidth\":1024}");

        Assert.Equal("zone-1:2026-11-13", result.RecordKey);
        Assert.Equal("zone-1", result.ZoneId);
        Assert.Equal(42, result.TotalRequests);
        Assert.Equal(1024, result.TotalBandwidthBytes);
        Assert.Contains("requests", result.PayloadJson);
    }

    [Fact]
    public void IdempotencyKeyIsStableForSameRunAndDataset()
    {
        var first = CloudflarePayloadNormalizer.IdempotencyKey("run-1", "daily", "zone-1", "2026-11-13");
        var second = CloudflarePayloadNormalizer.IdempotencyKey("run-1", "daily", "zone-1", "2026-11-13");

        Assert.Equal(first, second);
        Assert.Equal(64, first.Length);
    }

    [Theory]
    [InlineData(429, true)]
    [InlineData(503, true)]
    [InlineData(401, false)]
    public void CollectorClassifiesProviderFailures(int statusCode, bool retryable)
        => Assert.Equal(retryable, CloudflareCollector.IsRetryableStatus(statusCode));
}
