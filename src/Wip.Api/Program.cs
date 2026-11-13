using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Wip.Api.Contracts.V1;
using Wip.Persistence;

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

v1.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy", "Wip.Api", "v1")))
    .WithName("GetHealthV1")
    .WithTags("Health")
    .Produces<HealthResponse>(StatusCodes.Status200OK);

app.Run();

public partial class Program;
