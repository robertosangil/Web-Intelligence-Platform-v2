using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Tests;

public sealed class WpUmbrellaPersistenceModelTests
{
    public static IEnumerable<object[]> WpUmbrellaTables =>
    [
        [typeof(WpUmbrellaApiEvidence), "WpUmbrellaApiEvidence"],
        [typeof(WpUmbrellaAttentionItem), "WpUmbrellaAttentionItems"],
        [typeof(WpUmbrellaBackup), "WpUmbrellaBackups"],
        [typeof(WpUmbrellaChange), "WpUmbrellaChanges"],
        [typeof(WpUmbrellaDailySummary), "WpUmbrellaDailySummaries"],
        [typeof(WpUmbrellaPhpInfo), "WpUmbrellaPhpInfo"],
        [typeof(WpUmbrellaPhpIssue), "WpUmbrellaPhpIssues"],
        [typeof(WpUmbrellaPlugin), "WpUmbrellaPlugins"],
        [typeof(WpUmbrellaProcess), "WpUmbrellaProcesses"],
        [typeof(WpUmbrellaProjectSnapshot), "WpUmbrellaProjectSnapshots"],
        [typeof(WpUmbrellaTheme), "WpUmbrellaThemes"],
        [typeof(WpUmbrellaUptime), "WpUmbrellaUptime"],
        [typeof(WpUmbrellaUptimeIncident), "WpUmbrellaUptimeIncidents"],
        [typeof(WpUmbrellaVulnerability), "WpUmbrellaVulnerabilities"]
    ];

    [Theory]
    [MemberData(nameof(WpUmbrellaTables))]
    public void EveryWpUmbrellaDatasetHasAnExplicitTableAndUniqueKey(Type entityType, string tableName)
    {
        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True")
            .Options;
        using var context = new WipDbContext(options);
        var entity = context.Model.FindEntityType(entityType)!;

        Assert.Equal(tableName, entity.GetTableName());
        Assert.Contains(entity.GetIndexes(), index => index.IsUnique);
        Assert.NotNull(entity.FindProperty("PayloadJson"));
    }
}
