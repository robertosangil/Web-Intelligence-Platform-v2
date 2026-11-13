using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wip.Persistence.Identity;

namespace Wip.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WipDatabase");

        // Keep the context registered for all hosts so Identity's EF stores can be
        // composed during startup. The fallback is development-only and is not a
        // production connection or credential.
        var effectiveConnectionString = string.IsNullOrWhiteSpace(connectionString)
            ? "Server=(localdb)\\MSSQLLocalDB;Database=WipV2;Trusted_Connection=True;TrustServerCertificate=True"
            : connectionString;

        services.AddDbContext<WipDbContext>(options => options.UseSqlServer(effectiveConnectionString));

        services
            .AddIdentityCore<WipUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = configuration.GetValue("Identity:Password:RequiredLength", 12);
                options.Password.RequireDigit = configuration.GetValue("Identity:Password:RequireDigit", true);
                options.Password.RequireLowercase = configuration.GetValue("Identity:Password:RequireLowercase", true);
                options.Password.RequireUppercase = configuration.GetValue("Identity:Password:RequireUppercase", true);
                options.Password.RequireNonAlphanumeric = configuration.GetValue("Identity:Password:RequireNonAlphanumeric", true);
                options.Password.RequiredUniqueChars = configuration.GetValue("Identity:Password:RequiredUniqueChars", 1);
                options.Lockout.MaxFailedAccessAttempts = configuration.GetValue("Identity:Lockout:MaxFailedAccessAttempts", 5);
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
                    configuration.GetValue("Identity:Lockout:DurationMinutes", 15));
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<WipRole>()
            .AddEntityFrameworkStores<WipDbContext>();

        return services;
    }
}
