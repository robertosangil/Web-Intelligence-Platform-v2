using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Authorization;

public sealed class ModuleAccessRequirement(string moduleKey) : IAuthorizationRequirement
{
    public string ModuleKey { get; } = moduleKey;
}

public sealed class ModuleAccessHandler(WipDbContext db) : AuthorizationHandler<ModuleAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModuleAccessRequirement requirement)
    {
        var subject = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub");
        if (!Guid.TryParse(subject, out var userId)) return;

        var hasAccess = await (
            from userRole in db.UserRoles
            join user in db.Users on userRole.UserId equals user.Id
            join roleModule in db.RoleModules on userRole.RoleId equals roleModule.RoleId
            join module in db.Modules on roleModule.ModuleId equals module.Id
            where userRole.UserId == userId && user.IsActive && module.Key == requirement.ModuleKey
            select module.Id).AnyAsync();

        if (hasAccess) context.Succeed(requirement);
    }
}

public static class ModuleAuthorizationExtensions
{
    public static TBuilder RequireModuleAccess<TBuilder>(this TBuilder builder, string moduleKey)
        where TBuilder : IEndpointConventionBuilder
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(moduleKey);
        return builder.RequireAuthorization(policy => policy.AddRequirements(new ModuleAccessRequirement(moduleKey)));
    }
}
