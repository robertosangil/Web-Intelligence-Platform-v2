using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Wip.Application.Email;
using Wip.Infrastructure.Email;

namespace Wip.Worker.Tests;

public sealed class Smtp2GoEmailSenderTests
{
    [Fact]
    public async Task SendAsync_PostsProviderPayloadAndReturnsProviderMessageId()
    {
        var handler = new CapturingHandler();
        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.smtp2go.test/") };
        var sender = new Smtp2GoEmailSender(client, new Smtp2GoOptions { ApiKey = "test-key", FromEmail = "no-reply@example.com" });

        var result = await sender.SendAsync(new EmailMessage(
            "recipient@example.com", "Subject", "Plain body", "<p>Html body</p>"), CancellationToken.None);

        Assert.Equal("provider-123", result.ProviderMessageId);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("v3/email/send", handler.RequestUri!.PathAndQuery.TrimStart('/'));
        Assert.Equal("Bearer test-key", handler.Authorization);
        Assert.Equal("no-reply@example.com", handler.Payload!.Value.GetProperty("sender").GetString());
        Assert.Equal("recipient@example.com", handler.Payload.Value.GetProperty("to")[0].GetString());
        Assert.Equal("Plain body", handler.Payload.Value.GetProperty("text_body").GetString());
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        public HttpMethod? Method { get; private set; }
        public Uri? RequestUri { get; private set; }
        public string? Authorization { get; private set; }
        public JsonElement? Payload { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Method = request.Method;
            RequestUri = request.RequestUri;
            Authorization = request.Headers.Authorization?.ToString();
            Payload = await request.Content!.ReadFromJsonAsync<JsonElement>(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { data = new { email_id = "provider-123" } })
            };
        }
    }
}
