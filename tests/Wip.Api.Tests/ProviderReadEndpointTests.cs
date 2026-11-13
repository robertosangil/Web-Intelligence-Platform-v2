using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Wip.Api.Tests;

public sealed class ProviderReadEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProviderReadEndpointTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Theory]
    [InlineData("/api/v1/provider/cloudflare/daily")]
    [InlineData("/api/v1/provider/godaddy/domains")]
    [InlineData("/api/v1/provider/cpanel/resources")]
    [InlineData("/api/v1/provider/wp-umbrella/projects")]
    public async Task ProviderReadsRequireAuthentication(string path)
    {
        var response = await _client.GetAsync(path);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
