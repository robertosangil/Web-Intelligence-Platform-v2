using Wip.Worker;

namespace Wip.Worker.Tests;

public sealed class GoDaddyCollectorTests
{
    [Fact]
    public void NormalizerCreatesStableDomainSnapshot()
    {
        var result = GoDaddyPayloadNormalizer.NormalizeDomain(
            "example.com",
            "{\"status\":\"ACTIVE\",\"expiresAt\":\"2027-01-01T00:00:00Z\",\"autoRenew\":true}");

        Assert.Equal("example.com", result.DomainKey);
        Assert.Equal("ACTIVE", result.Status);
        Assert.True(result.AutoRenew);
        Assert.Equal("example.com", result.Domain);
    }

    [Fact]
    public void ProviderKeyIsStableForReplay()
    {
        var first = GoDaddyPayloadNormalizer.ProviderKey("domains", "example.com");
        var second = GoDaddyPayloadNormalizer.ProviderKey("domains", "example.com");

        Assert.Equal(first, second);
        Assert.Equal(64, first.Length);
    }

    [Theory]
    [InlineData(429, true)]
    [InlineData(503, true)]
    [InlineData(401, false)]
    public void CollectorClassifiesProviderFailures(int statusCode, bool retryable)
        => Assert.Equal(retryable, GoDaddyCollector.IsRetryableStatus(statusCode));
}
