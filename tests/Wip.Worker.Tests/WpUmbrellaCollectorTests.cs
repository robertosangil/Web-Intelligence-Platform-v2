using Wip.Worker;

namespace Wip.Worker.Tests;

public sealed class WpUmbrellaCollectorTests
{
    [Fact]
    public void NormalizerCreatesStableProjectSnapshot()
    {
        var result = WpUmbrellaPayloadNormalizer.NormalizeProject(
            "site-1", "Example", "{\"projectId\":\"p-1\",\"projectName\":\"Main\",\"status\":\"active\"}");

        Assert.Equal("site-1:p-1", result.ProjectKey);
        Assert.Equal("p-1", result.ProjectId);
        Assert.Equal("Main", result.ProjectName);
        Assert.Equal("active", result.Status);
    }

    [Fact]
    public void ProviderKeyIsStableForReplay()
    {
        var first = WpUmbrellaPayloadNormalizer.ProviderKey("projects", "site-1", "p-1");
        var second = WpUmbrellaPayloadNormalizer.ProviderKey("projects", "site-1", "p-1");

        Assert.Equal(first, second);
        Assert.Equal(64, first.Length);
    }

    [Theory]
    [InlineData(429, true)]
    [InlineData(500, true)]
    [InlineData(403, false)]
    public void CollectorClassifiesProviderFailures(int statusCode, bool retryable)
        => Assert.Equal(retryable, WpUmbrellaCollector.IsRetryableStatus(statusCode));
}
