using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wip.Api.Contracts.V1;

namespace Wip.Api.Tests;

public sealed class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealthReturnsVersionedHealthyContract()
    {
        var response = await _client.GetAsync("/api/v1/health");
        var contract = await response.Content.ReadFromJsonAsync<HealthResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(contract);
        Assert.Equal("Healthy", contract.Status);
        Assert.Equal("Wip.Api", contract.Service);
        Assert.Equal("v1", contract.ApiVersion);
    }
}
