namespace Wip.Api.Contracts.V1;

public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshRequest(string RefreshToken);
public sealed record TokenResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

public sealed record ProfileResponse(Guid Id, string Email, string? FirstName, string? LastName, bool IsActive, DateTime? DeactivatedAtUtc);
public sealed record UpdateProfileRequest(string? FirstName, string? LastName);
public sealed record PermissionsResponse(IReadOnlyList<string> Roles, IReadOnlyList<PermissionModuleResponse> Modules);
public sealed record PermissionModuleResponse(string Key, string Name, string GroupKey, int SortOrder);
