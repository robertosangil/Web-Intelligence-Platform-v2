using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Wip.Application.Email;

namespace Wip.Infrastructure.Email;

public sealed class Smtp2GoEmailSender(HttpClient httpClient, Smtp2GoOptions options) : IEmailSender
{
    private readonly Smtp2GoOptions _options = options;

    public async Task<EmailSendResult> SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "v3/email/send");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        request.Content = JsonContent.Create(new Smtp2GoRequest(
            _options.FromEmail, [message.To], message.Subject, message.TextBody, message.HtmlBody));

        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<Smtp2GoResponse>(cancellationToken);
        var messageId = payload?.Data?.EmailId;
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new InvalidOperationException("SMTP2Go response did not contain an email identifier.");
        }

        return new EmailSendResult(messageId);
    }

    private sealed record Smtp2GoRequest(
        [property: JsonPropertyName("sender")] string Sender,
        [property: JsonPropertyName("to")] IReadOnlyList<string> To,
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("text_body")] string TextBody,
        [property: JsonPropertyName("html_body")] string? HtmlBody);

    private sealed record Smtp2GoResponse([property: JsonPropertyName("data")] ResponseData? Data);

    private sealed record ResponseData([property: JsonPropertyName("email_id")] string? EmailId);
}
