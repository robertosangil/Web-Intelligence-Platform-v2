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

static string CreateToken(WipUser user, IConfiguration config, DateTime now, out DateTime expires)
{
    var jwt = config.GetSection("Jwt");
    expires = now.AddMinutes(jwt.GetValue<int?>("AccessTokenMinutes") ?? 10);
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SigningKey"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email ?? string.Empty) };
    return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwt["Issuer"], jwt["Audience"], claims, now, expires, credentials));
}
static async Task<TokenResponse> IssueTokens(WipUser user, WipDbContext db, IConfiguration config, CancellationToken ct)
{
    var now = DateTime.UtcNow;
    var access = CreateToken(user, config, now, out var accessExpiry);
    var refreshExpiry = now.AddDays(config.GetValue<int?>("Jwt:RefreshTokenDays") ?? 30);
    var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    db.RefreshSessions.Add(new RefreshSession { Id = Guid.NewGuid(), UserId = user.Id, TokenHash = AuthCrypto.HashRefresh(raw), CreatedAtUtc = now, ExpiresAtUtc = refreshExpiry });
    await db.SaveChangesAsync(ct);
    return new TokenResponse(access, raw, accessExpiry, refreshExpiry);
}

v1.MapPost("/auth/login", async (LoginRequest request, UserManager<WipUser> users, WipDbContext db, IConfiguration config, CancellationToken ct) =>
{
    var user = await users.FindByEmailAsync(request.Email);
    if (user is null || await users.IsLockedOutAsync(user)) return Results.Unauthorized();
    var valid = await users.CheckPasswordAsync(user, request.Password);
    if (!valid)
    {
        await users.AccessFailedAsync(user);
        return Results.Unauthorized();
    }

    await users.ResetAccessFailedCountAsync(user);
    return Results.Ok(await IssueTokens(user, db, config, ct));
}).WithTags("Authentication");

v1.MapPost("/auth/refresh", async (RefreshRequest request, UserManager<WipUser> users, WipDbContext db, IConfiguration config, CancellationToken ct) =>
{
    var session = await db.RefreshSessions.SingleOrDefaultAsync(x => x.TokenHash == AuthCrypto.HashRefresh(request.RefreshToken), ct);
    if (session is null || session.RevokedAtUtc is not null || session.ExpiresAtUtc <= DateTime.UtcNow) return Results.Unauthorized();
    var user = await users.FindByIdAsync(session.UserId.ToString());
    if (user is null || await users.IsLockedOutAsync(user)) return Results.Unauthorized();
    session.RevokedAtUtc = DateTime.UtcNow;
    var response = await IssueTokens(user, db, config, ct);
    session.ReplacedBySessionId = await db.RefreshSessions
        .Where(x => x.UserId == user.Id && x.CreatedAtUtc >= session.RevokedAtUtc)
        .OrderByDescending(x => x.CreatedAtUtc)
        .Select(x => x.Id)
        .FirstAsync(ct);
    await db.SaveChangesAsync(ct);
    return Results.Ok(response);
}).WithTags("Authentication");

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
