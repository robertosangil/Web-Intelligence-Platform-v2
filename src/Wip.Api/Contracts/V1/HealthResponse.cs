namespace Wip.Api.Contracts.V1;

public sealed record HealthResponse(string Status, string Service, string ApiVersion);
