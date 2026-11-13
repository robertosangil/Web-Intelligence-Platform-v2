namespace Wip.Infrastructure.Email;

public sealed class Smtp2GoOptions
{
    public const string SectionName = "Smtp2Go";

    public string BaseUrl { get; set; } = "https://api.smtp2go.com/";
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
}
