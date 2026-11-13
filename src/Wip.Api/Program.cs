using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wip.Api.Contracts.V1;
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
builder.Services.AddAuthorization();
builder.Services.AddPersistence(builder.Configuration);

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

static Guid? GetUserId(ClaimsPrincipal principal)
{
    var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
    return Guid.TryParse(value, out var id) ? id : null;
}

static ProfileResponse ToProfile(WipUser user) =>
    new(user.Id, user.Email ?? string.Empty, user.FirstName, user.LastName, user.IsActive, user.DeactivatedAtUtc);

var profile = v1.MapGroup("/me").RequireAuthorization().WithTags("Profile");

profile.MapGet("", async (ClaimsPrincipal principal, UserManager<WipUser> users, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    return user is null ? Results.NotFound() : Results.Ok(ToProfile(user));
});

profile.MapPut("", async (UpdateProfileRequest request, ClaimsPrincipal principal, UserManager<WipUser> users, CancellationToken ct) =>
{
    var id = GetUserId(principal);
    if (id is null) return Results.Unauthorized();
    var user = await users.FindByIdAsync(id.Value.ToString());
    if (user is null) return Results.NotFound();
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

v1.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy", "Wip.Api", "v1")))
    .WithName("GetHealthV1")
    .WithTags("Health")
    .Produces<HealthResponse>(StatusCodes.Status200OK);

app.Run();

public partial class Program;
