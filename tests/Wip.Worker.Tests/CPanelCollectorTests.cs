using Wip.Worker;

namespace Wip.Worker.Tests;

public sealed class CPanelCollectorTests
{
    [Fact]
    public void NormalizerCreatesStableResourceSnapshot()
    {
        var result = CPanelPayloadNormalizer.NormalizeResource(
            "cp-user", "disk", "{\"current\":12.5,\"limit\":100,\"unit\":\"GB\",\"status\":\"ok\"}");

        Assert.Equal("cp-user:disk", result.ResourceKey);
        Assert.Equal(12.5m, result.CurrentValue);
        Assert.Equal(100m, result.LimitValue);
        Assert.Equal("GB", result.Unit);
    }

    [Fact]
    public void ProviderKeyIsStableForDuplicateResourceEvents()
    {
        var first = CPanelPayloadNormalizer.ProviderKey("resources", "cp-user", "disk");
        var second = CPanelPayloadNormalizer.ProviderKey("resources", "cp-user", "disk");

        Assert.Equal(first, second);
        Assert.Equal(64, first.Length);
    }

    [Theory]
    [InlineData(429, true)]
    [InlineData(502, true)]
    [InlineData(400, false)]
    public void CollectorClassifiesProviderFailures(int statusCode, bool retryable)
        => Assert.Equal(retryable, CPanelCollector.IsRetryableStatus(statusCode));
}
