using Wip.Api.Contracts.V1;
using Wip.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1");
builder.Services.AddPersistence(builder.Configuration);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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
