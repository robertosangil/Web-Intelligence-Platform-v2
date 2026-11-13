using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Tests;

public sealed class CPanelPersistenceModelTests
{
    public static IEnumerable<object[]> CPanelTables =>
    [
        [typeof(CPanelApiEvidence), "CPanelApiEvidence"],
        [typeof(CPanelAutoSsl), "CPanelAutoSsl"],
        [typeof(CPanelBackup), "CPanelBackups"],
        [typeof(CPanelCapability), "CPanelCapabilities"],
        [typeof(CPanelDailyStat), "CPanelDailyStats"],
        [typeof(CPanelDatabase), "CPanelDatabases"],
        [typeof(CPanelResourceDailySummary), "CPanelResourceDailySummaries"],
        [typeof(CPanelResourceSampleLegacy), "CPanelResourceSamplesLegacy"],
        [typeof(CPanelResource), "CPanelResources"],
        [typeof(CPanelServerError), "CPanelServerErrors"],
        [typeof(CPanelServerInfo), "CPanelServerInfo"],
        [typeof(CPanelSslCertificate), "CPanelSslCertificates"],
        [typeof(CPanelTransaction), "CPanelTransactions"]
    ];

    [Theory]
    [MemberData(nameof(CPanelTables))]
    public void EveryCPanelDatasetHasAnExplicitTableAndUniqueKey(Type entityType, string tableName)
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
