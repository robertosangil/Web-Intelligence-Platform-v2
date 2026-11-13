namespace Wip.Application.Email;

public sealed record EmailMessage(string To, string Subject, string TextBody, string? HtmlBody = null);

public sealed record EmailSendResult(string ProviderMessageId);

public interface IEmailSender
{
    Task<EmailSendResult> SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
