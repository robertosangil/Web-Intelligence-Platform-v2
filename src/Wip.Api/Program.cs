using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Wip.Api.Contracts.V1;
using Wip.Api.Authorization;
using Wip.Application.Email;
using Wip.Infrastructure.Email;
using Wip.Persistence;
using Wip.Persistence.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["SigningKey"];
if (!builder.Environment.IsDevelopment() && string.IsNullOrWhiteSpace(signingKey))
{
    throw new InvalidOperationException("Jwt:SigningKey must be configured outside Development.");
}
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(jwtSection.GetValue<int?>("ClockSkewSeconds") ?? 30),
            ValidateIssuerSigningKey = !string.IsNullOrWhiteSpace(signingKey),
            IssuerSigningKey = string.IsNullOrWhiteSpace(signingKey)
                ? null
                : new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey))
        };
    });
builder.Services.AddAuthorization(options => options.AddPolicy("Administrator", policy =>
    policy.RequireAuthenticatedUser().RequireRole("Administrator", "Super Admin")));
builder.Services.AddScoped<IAuthorizationHandler, ModuleAccessHandler>();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.Configure<Smtp2GoOptions>(builder.Configuration.GetSection(Smtp2GoOptions.SectionName));
builder.Services.AddSingleton(builder.Configuration.GetSection(Smtp2GoOptions.SectionName).Get<Smtp2GoOptions>() ?? new Smtp2GoOptions());
builder.Services.AddHttpClient<IEmailSender, Smtp2GoEmailSender>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<Smtp2GoOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
}

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Wip.Api v1");
        options.RoutePrefix = "swagger";
    });
}

if (allowedOrigins.Length > 0)
{
    app.UseCors();
}

var v1 = app.MapGroup("/api/v1");

static string CreateToken(WipUser user, IEnumerable<string> roles, IConfiguration config, DateTime now, out DateTime expires)
{
    var jwt = config.GetSection("Jwt");
    expires = now.AddMinutes(jwt.GetValue<int?>("AccessTokenMinutes") ?? 10);
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SigningKey"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new(ClaimTypes.Email, user.Email ?? string.Empty) };
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwt["Issuer"], jwt["Audience"], claims, now, expires, credentials));
}
static async Task<TokenResponse> IssueTokens(WipUser user, UserManager<WipUser> users, WipDbContext db, IConfiguration config, CancellationToken ct)
{
    var now = DateTime.UtcNow;
    var roles = await users.GetRolesAsync(user);
    var access = CreateToken(user, roles, config, now, out var accessExpiry);
    var refreshExpiry = now.AddDays(config.GetValue<int?>("Jwt:RefreshTokenDays") ?? 30);
    var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    db.RefreshSessions.Add(new RefreshSession { Id = Guid.NewGuid(), UserId = user.Id, TokenHash = AuthCrypto.HashRefresh(raw), CreatedAtUtc = now, ExpiresAtUtc = refreshExpiry });
    await db.SaveChangesAsync(ct);
    return new TokenResponse(access, raw, accessExpiry, refreshExpiry);
}

static string HashResetToken(string token) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(token)));

v1.MapPost("/auth/login", async (LoginRequest request, UserManager<WipUser> users, WipDbContext db, IConfiguration config, CancellationToken ct) =>
{
    var user = await users.FindByEmailAsync(request.Email);
    if (user is null || !user.IsActive || await users.IsLockedOutAsync(user)) return Results.Unauthorized();
    var valid = await users.CheckPasswordAsync(user, request.Password);
    if (!valid)
    {
        await users.AccessFailedAsync(user);
        return Results.Unauthorized();
    }

    await users.ResetAccessFailedCountAsync(user);
    return Results.Ok(await IssueTokens(user, users, db, config, ct));
}).WithTags("Authentication");

v1.MapPost("/auth/refresh", async (RefreshRequest request, UserManager<WipUser> users, WipDbContext db, IConfiguration config, CancellationToken ct) =>
{
    var session = await db.RefreshSessions.SingleOrDefaultAsync(x => x.TokenHash == AuthCrypto.HashRefresh(request.RefreshToken), ct);
    if (session is null || session.RevokedAtUtc is not null || session.ExpiresAtUtc <= DateTime.UtcNow) return Results.Unauthorized();
    var user = await users.FindByIdAsync(session.UserId.ToString());
    if (user is null || !user.IsActive || await users.IsLockedOutAsync(user)) return Results.Unauthorized();
    session.RevokedAtUtc = DateTime.UtcNow;
    var response = await IssueTokens(user, users, db, config, ct);
    session.ReplacedBySessionId = await db.RefreshSessions
        .Where(x => x.UserId == user.Id && x.CreatedAtUtc >= session.RevokedAtUtc)
        .OrderByDescending(x => x.CreatedAtUtc)
        .Select(x => x.Id)
        .FirstAsync(ct);
    await db.SaveChangesAsync(ct);
    return Results.Ok(response);
}).WithTags("Authentication");

v1.MapPost("/auth/forgot-password", async (ForgotPasswordRequest request, UserManager<WipUser> users, WipDbContext db, IEmailSender emailSender, IConfiguration config, CancellationToken ct) =>
{
    var user = await users.FindByEmailAsync(request.Email.Trim());
    if (user is not null && user.IsActive)
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        var now = DateTime.UtcNow;
        db.PasswordResetTokens.Add(new PasswordResetToken { Id = Guid.NewGuid(), UserId = user.Id, TokenHash = HashResetToken(raw), Purpose = "password-reset", CreatedAtUtc = now, ExpiresAtUtc = now.AddMinutes(config.GetValue<int?>("PasswordReset:TokenExpiryMinutes") ?? 30) });
        await db.SaveChangesAsync(ct);
        var resetPath = config["PasswordReset:ResetPath"] ?? "/reset-password";
        var body = $"Use the password reset path {resetPath} with the one-time token supplied by this message.";
        try { await emailSender.SendAsync(new EmailMessage(user.Email!, "WIP v2 password reset", body), ct); } catch { /* do not reveal provider/account state */ }
    }
    return Results.Accepted();
}).WithTags("Authentication");

v1.MapPost("/auth/reset-password", async (ResetPasswordRequest request, UserManager<WipUser> users, WipDbContext db, CancellationToken ct) =>
{
    var user = await users.FindByEmailAsync(request.Email.Trim());
    if (user is null || !user.IsActive) return Results.BadRequest(new { error = "The password reset request is invalid or expired." });
    var tokenHash = HashResetToken(request.Token);
    var token = await db.PasswordResetTokens.SingleOrDefaultAsync(x => x.UserId == user.Id && x.Purpose == "password-reset" && x.TokenHash == tokenHash && x.UsedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow, ct);
    if (token is null) return Results.BadRequest(new { error = "The password reset request is invalid or expired." });
    var result = await users.ResetPasswordAsync(user, request.Token, request.NewPassword);
    if (!result.Succeeded) return Results.BadRequest(new { error = "The password reset request is invalid or expired." });
    token.UsedAtUtc = DateTime.UtcNow;
    await users.UpdateSecurityStampAsync(user);
    await db.RefreshSessions.Where(x => x.UserId == user.Id && x.RevokedAtUtc == null).ExecuteUpdateAsync(x => x.SetProperty(y => y.RevokedAtUtc, DateTime.UtcNow), ct);
    await db.SaveChangesAsync(ct);
    return Results.NoContent();
}).WithTags("Authentication");

static Guid? GetUserId(ClaimsPrincipal principal)
{
    var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
    return Guid.TryParse(value, out var id) ? id : null;
}

static ProfileResponse ToProfile(WipUser user) =>
    new(user.Id, user.Email ?? string.Empty, user.FirstName, user.LastName, user.IsActive, user.DeactivatedAtUtc);

var profile = v1.MapGroup("/me").RequireAuthorization().WithTags("Profile");

profile.MapPost("/password", async (ChangePasswordRequest request, ClaimsPrincipal principal, UserManager<WipUser> users, WipDbContext db, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    if (user is null || !user.IsActive) return Results.Unauthorized();
    var result = await users.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
    if (!result.Succeeded) return Results.BadRequest(new { error = "The current password is incorrect or the new password is invalid." });
    await users.UpdateSecurityStampAsync(user);
    await db.RefreshSessions.Where(x => x.UserId == user.Id && x.RevokedAtUtc == null)
        .ExecuteUpdateAsync(x => x.SetProperty(y => y.RevokedAtUtc, DateTime.UtcNow), ct);
    return Results.NoContent();
}).WithName("ChangeMyPassword");

profile.MapGet("", async (ClaimsPrincipal principal, UserManager<WipUser> users, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    return user is null ? Results.NotFound() : !user.IsActive ? Results.Unauthorized() : Results.Ok(ToProfile(user));
});

profile.MapPut("", async (UpdateProfileRequest request, ClaimsPrincipal principal, UserManager<WipUser> users, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    if (user is null) return Results.NotFound();
    if (!user.IsActive) return Results.Unauthorized();
    user.FirstName = request.FirstName?.Trim();
    user.LastName = request.LastName?.Trim();
    var result = await users.UpdateAsync(user);
    return result.Succeeded ? Results.Ok(ToProfile(user)) : Results.ValidationProblem(result.Errors
        .GroupBy(error => error.Code)
        .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));
});

profile.MapDelete("", async (ClaimsPrincipal principal, UserManager<WipUser> users, WipDbContext db, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    if (user is null) return Results.NotFound();
    if (user.IsActive)
    {
        user.IsActive = false;
        user.DeactivatedAtUtc = DateTime.UtcNow;
        var result = await users.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Results.ValidationProblem(result.Errors
                .GroupBy(error => error.Code)
                .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));
        }
        await db.RefreshSessions.Where(session => session.UserId == user.Id && session.RevokedAtUtc == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(session => session.RevokedAtUtc, DateTime.UtcNow), ct);
    }
    return Results.NoContent();
});

profile.MapGet("/permissions", async (ClaimsPrincipal principal, UserManager<WipUser> users, WipDbContext db, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    if (user is null) return Results.NotFound();
    if (!user.IsActive) return Results.Unauthorized();

    var roles = await users.GetRolesAsync(user);
    var modules = (await (
        from role in db.Roles
        join userRole in db.UserRoles on role.Id equals userRole.RoleId
        join roleModule in db.RoleModules on role.Id equals roleModule.RoleId
        join module in db.Modules on roleModule.ModuleId equals module.Id
        where userRole.UserId == user.Id
        select new PermissionModuleResponse(module.Key, module.Name, module.GroupKey, module.SortOrder))
        .ToListAsync(ct))
        .Distinct()
        .OrderBy(module => module.GroupKey)
        .ThenBy(module => module.SortOrder)
        .ToList();

    return Results.Ok(new PermissionsResponse(roles.ToArray(), modules));
}).WithName("GetMyPermissions");

v1.MapPost("/auth/logout", async (RefreshRequest request, WipDbContext db, CancellationToken ct) =>
{
    var session = await db.RefreshSessions.SingleOrDefaultAsync(x => x.TokenHash == AuthCrypto.HashRefresh(request.RefreshToken), ct);
    if (session is not null && session.RevokedAtUtc is null) { session.RevokedAtUtc = DateTime.UtcNow; await db.SaveChangesAsync(ct); }
    return Results.NoContent();
}).WithTags("Authentication");

var admin = v1.MapGroup("/admin").RequireAuthorization("Administrator").WithTags("Administration");
static AdminUserResponse ToAdminUser(WipUser user, IEnumerable<string> roles) =>
    new(user.Id, user.Email ?? string.Empty, user.FirstName, user.LastName, user.IsActive, roles.ToArray());
static bool IsSuperAdmin(string? name) => string.Equals(name, "Super Admin", StringComparison.OrdinalIgnoreCase);

admin.MapGet("/modules", async (WipDbContext db, CancellationToken ct) => Results.Ok(await db.Modules.AsNoTracking()
    .OrderBy(x => x.GroupKey).ThenBy(x => x.SortOrder).Select(x => new AdminModuleResponse(x.Id, x.Key, x.Name, x.GroupKey, x.SortOrder)).ToListAsync(ct)));
admin.MapGet("/users", async (UserManager<WipUser> users, CancellationToken ct) =>
{
    var result = new List<AdminUserResponse>();
    foreach (var user in await users.Users.AsNoTracking().OrderBy(x => x.Email).ToListAsync(ct))
        result.Add(ToAdminUser(user, await users.GetRolesAsync(user)));
    return Results.Ok(result);
});
admin.MapPost("/users", async (CreateAdminUserRequest request, ClaimsPrincipal principal, UserManager<WipUser> users, RoleManager<WipRole> roleManager, CancellationToken ct) =>
{
    var user = new WipUser { UserName = request.Email.Trim(), Email = request.Email.Trim(), FirstName = request.FirstName?.Trim(), LastName = request.LastName?.Trim() };
    var created = await users.CreateAsync(user, request.Password);
    if (!created.Succeeded) return Results.ValidationProblem(created.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray()));
    var roleNames = request.Roles?.Distinct(StringComparer.OrdinalIgnoreCase).ToArray() ?? [];
    if (roleNames.Any(IsSuperAdmin) && !principal.IsInRole("Super Admin")) return Results.Forbid();
    var knownRoles = await roleManager.Roles.Where(x => x.Name != null && roleNames.Contains(x.Name)).CountAsync(ct);
    if (knownRoles != roleNames.Length) return Results.NotFound(new { error = "One or more role names were not found." });
    if (roleNames.Length > 0) { var rr = await users.AddToRolesAsync(user, roleNames); if (!rr.Succeeded) return Results.ValidationProblem(rr.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray())); }
    return Results.Created($"/api/v1/admin/users/{user.Id}", ToAdminUser(user, roleNames));
});
admin.MapPut("/users/{id:guid}", async (Guid id, UpdateAdminUserRequest request, ClaimsPrincipal principal, UserManager<WipUser> users, WipDbContext db, CancellationToken ct) =>
{
    var user = await users.FindByIdAsync(id.ToString()); if (user is null) return Results.NotFound();
    var existing = await users.GetRolesAsync(user); var requested = request.Roles?.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    if (requested is not null && requested.Any(IsSuperAdmin) && !principal.IsInRole("Super Admin")) return Results.Forbid();
    if (requested is not null && existing.Any(IsSuperAdmin) && !requested.Any(IsSuperAdmin)) return Results.Conflict(new { error = "The Super Admin role cannot be removed." });
    if (request.IsActive == false && existing.Any(IsSuperAdmin)) return Results.Conflict(new { error = "A Super Admin cannot be deactivated." });
    user.FirstName = request.FirstName?.Trim() ?? user.FirstName; user.LastName = request.LastName?.Trim() ?? user.LastName;
    if (request.IsActive.HasValue) { user.IsActive = request.IsActive.Value; user.DeactivatedAtUtc = user.IsActive ? null : DateTime.UtcNow; }
    var updated = await users.UpdateAsync(user); if (!updated.Succeeded) return Results.ValidationProblem(updated.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray()));
    if (requested is not null) { var removing = existing.Except(requested, StringComparer.OrdinalIgnoreCase); var adding = requested.Except(existing, StringComparer.OrdinalIgnoreCase); if (removing.Any()) await users.RemoveFromRolesAsync(user, removing); if (adding.Any()) await users.AddToRolesAsync(user, adding); }
    if (!user.IsActive) await db.RefreshSessions.Where(x => x.UserId == user.Id && x.RevokedAtUtc == null).ExecuteUpdateAsync(x => x.SetProperty(y => y.RevokedAtUtc, DateTime.UtcNow), ct);
    return Results.Ok(ToAdminUser(user, await users.GetRolesAsync(user)));
});
admin.MapGet("/roles", async (RoleManager<WipRole> roles, WipDbContext db, CancellationToken ct) => Results.Ok(await roles.Roles.AsNoTracking().Select(r => new AdminRoleResponse(r.Id, r.Name!, db.RoleModules.Where(rm => rm.RoleId == r.Id).Join(db.Modules, rm => rm.ModuleId, m => m.Id, (rm, m) => m.Key).ToList())).ToListAsync(ct)));
admin.MapPost("/roles", async (CreateAdminRoleRequest request, RoleManager<WipRole> roles, WipDbContext db, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || IsSuperAdmin(request.Name)) return Results.BadRequest(new { error = "Invalid or protected role name." });
    var moduleIds = request.ModuleIds?.Distinct().ToArray() ?? [];
    if (moduleIds.Length != await db.Modules.CountAsync(x => moduleIds.Contains(x.Id), ct)) return Results.NotFound(new { error = "One or more module identifiers were not found." });
    var role = new WipRole { Name = request.Name.Trim() }; var result = await roles.CreateAsync(role); if (!result.Succeeded) return Results.ValidationProblem(result.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray()));
    foreach (var moduleId in moduleIds) db.RoleModules.Add(new RoleModule { RoleId = role.Id, ModuleId = moduleId });
    await db.SaveChangesAsync(ct); return Results.Created($"/api/v1/admin/roles/{role.Id}", new AdminRoleResponse(role.Id, role.Name!, await db.Modules.Where(x => moduleIds.Contains(x.Id)).Select(x => x.Key).ToListAsync(ct)));
});
admin.MapPut("/roles/{id:guid}/modules", async (Guid id, UpdateAdminRoleRequest request, RoleManager<WipRole> roles, WipDbContext db, CancellationToken ct) =>
{
    var role = await roles.FindByIdAsync(id.ToString()); if (role is null) return Results.NotFound(); if (IsSuperAdmin(role.Name)) return Results.Conflict(new { error = "The Super Admin role is protected." });
    var ids = request.ModuleIds?.Distinct().ToHashSet() ?? [];
    if (ids.Count != await db.Modules.CountAsync(x => ids.Contains(x.Id), ct)) return Results.NotFound(new { error = "One or more module identifiers were not found." });
    var links = await db.RoleModules.Where(x => x.RoleId == id).ToListAsync(ct); db.RoleModules.RemoveRange(links); foreach (var moduleId in ids) db.RoleModules.Add(new RoleModule { RoleId = id, ModuleId = moduleId }); await db.SaveChangesAsync(ct); return Results.NoContent();
});
admin.MapPut("/users/{userId:guid}/roles/{roleId:guid}", async (Guid userId, Guid roleId, ClaimsPrincipal principal, UserManager<WipUser> users, RoleManager<WipRole> roles, CancellationToken ct) =>
{
    var user = await users.FindByIdAsync(userId.ToString()); var role = await roles.FindByIdAsync(roleId.ToString()); if (user is null || role is null) return Results.NotFound();
    if (IsSuperAdmin(role.Name) && !principal.IsInRole("Super Admin")) return Results.Forbid();
    if (IsSuperAdmin(role.Name) && await users.IsInRoleAsync(user, role.Name!)) return Results.NoContent();
    var result = await users.AddToRoleAsync(user, role.Name!); return result.Succeeded ? Results.NoContent() : Results.ValidationProblem(result.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray()));
});
admin.MapDelete("/users/{userId:guid}/roles/{roleId:guid}", async (Guid userId, Guid roleId, ClaimsPrincipal principal, UserManager<WipUser> users, RoleManager<WipRole> roles, CancellationToken ct) =>
{
    var user = await users.FindByIdAsync(userId.ToString()); var role = await roles.FindByIdAsync(roleId.ToString()); if (user is null || role is null) return Results.NotFound();
    if (IsSuperAdmin(role.Name) && !principal.IsInRole("Super Admin")) return Results.Forbid();
    if (IsSuperAdmin(role.Name)) { var count = await users.GetUsersInRoleAsync(role.Name!); if (count.Count <= 1) return Results.Conflict(new { error = "At least one Super Admin must remain." }); }
    var result = await users.RemoveFromRoleAsync(user, role.Name!); return result.Succeeded ? Results.NoContent() : Results.ValidationProblem(result.Errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray()));
});

v1.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy", "Wip.Api", "v1")))
    .WithName("GetHealthV1")
    .WithTags("Health")
    .Produces<HealthResponse>(StatusCodes.Status200OK);

app.Run();

public partial class Program;
