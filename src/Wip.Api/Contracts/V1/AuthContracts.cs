namespace Wip.Api.Contracts.V1;

public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshRequest(string RefreshToken);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
public sealed record TokenResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

public sealed record ProfileResponse(Guid Id, string Email, string? FirstName, string? LastName, bool IsActive, DateTime? DeactivatedAtUtc);
public sealed record UpdateProfileRequest(string? FirstName, string? LastName);
public sealed record PermissionsResponse(IReadOnlyList<string> Roles, IReadOnlyList<PermissionModuleResponse> Modules);
public sealed record PermissionModuleResponse(string Key, string Name, string GroupKey, int SortOrder);

public sealed record AdminModuleResponse(Guid Id, string Key, string Name, string GroupKey, int SortOrder);
public sealed record AdminUserResponse(Guid Id, string Email, string? FirstName, string? LastName, bool IsActive, IReadOnlyList<string> Roles);
public sealed record CreateAdminUserRequest(string Email, string Password, string? FirstName, string? LastName, IReadOnlyList<string>? Roles);
public sealed record UpdateAdminUserRequest(string? FirstName, string? LastName, bool? IsActive, IReadOnlyList<string>? Roles);
public sealed record AdminRoleResponse(Guid Id, string Name, IReadOnlyList<string> Modules);
public sealed record CreateAdminRoleRequest(string Name, IReadOnlyList<Guid>? ModuleIds);
public sealed record UpdateAdminRoleRequest(IReadOnlyList<Guid>? ModuleIds);
public sealed record AssignmentRequest(Guid Id);
