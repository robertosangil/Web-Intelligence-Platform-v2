using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wip.Persistence;

public sealed class WipDbContextFactory : IDesignTimeDbContextFactory<WipDbContext>
{
    public WipDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__WipDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set ConnectionStrings__WipDatabase before using EF Core design-time commands.");
        }

        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new WipDbContext(options);
    }
}
