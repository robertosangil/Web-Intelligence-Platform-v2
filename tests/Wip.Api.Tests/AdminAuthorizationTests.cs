using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Wip.Api.Tests;

public sealed class AdminAuthorizationTests
{
    private static readonly string[] AdminEndpoints =
    [
        "/api/v1/admin/modules",
        "/api/v1/admin/users",
        "/api/v1/admin/roles"
    ];

    [Fact]
    public async Task AnonymousAdminRequestsReturnUnauthorized()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();

        foreach (var endpoint in AdminEndpoints)
        {
            var response = await client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

    [Fact]
    public async Task AuthenticatedNonAdministratorAdminRequestsReturnForbidden()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        foreach (var endpoint in AdminEndpoints)
        {
            var response = await client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    private static WebApplicationFactory<Program> CreateFactory() =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationHandler.Scheme;
                    options.DefaultChallengeScheme = TestAuthenticationHandler.Scheme;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.Scheme, _ => { });
            });
        });
}

internal sealed class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public new const string Scheme = "WipTests";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Test-Role", out var role) || string.IsNullOrWhiteSpace(role))
            return Task.FromResult(AuthenticateResult.NoResult());

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, "test-user")],
            Scheme,
            ClaimTypes.Name,
            ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme)));
    }
}
