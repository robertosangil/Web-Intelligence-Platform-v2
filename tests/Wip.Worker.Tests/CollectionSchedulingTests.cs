using Microsoft.Extensions.Options;
using Wip.Worker;

namespace Wip.Worker.Tests;

public sealed class CollectionSchedulingTests
{
    [Fact]
    public void DefaultScheduleContainsTheFourApprovedProviders()
    {
        var options = new CollectionSchedulerOptions();

        Assert.Equal(["Cloudflare", "GoDaddy", "cPanel", "WpUmbrella"],
            options.Definitions.Select(x => x.Provider).ToArray());
        Assert.Equal(4, options.MaxConcurrency);
    }

    [Fact]
    public void PlannerReturnsOnlyEnabledDefinitionsThatAreDue()
    {
        var now = new DateTimeOffset(2026, 11, 13, 6, 0, 0, TimeSpan.Zero);
        var definitions = new[]
        {
            new CollectionDefinition("Cloudflare", "daily", TimeSpan.FromHours(24), true),
            new CollectionDefinition("GoDaddy", "daily", TimeSpan.FromHours(24), false),
            new CollectionDefinition("cPanel", "daily", TimeSpan.FromHours(24), true)
        };

        var due = CollectionSchedulePlanner.GetDue(definitions, now, new Dictionary<string, DateTimeOffset>
        {
            ["Cloudflare:daily"] = now.AddHours(-25),
            ["cPanel:daily"] = now.AddHours(-2)
        });

        Assert.Single(due);
        Assert.Equal("Cloudflare", due[0].Provider);
    }

    [Theory]
    [InlineData(429, true)]
    [InlineData(408, true)]
    [InlineData(500, true)]
    [InlineData(400, false)]
    [InlineData(401, false)]
    public void RetryPolicyDistinguishesTransientHttpFailures(int statusCode, bool expected)
        => Assert.Equal(expected, CollectionRetryPolicy.IsRetryable(statusCode));
}
