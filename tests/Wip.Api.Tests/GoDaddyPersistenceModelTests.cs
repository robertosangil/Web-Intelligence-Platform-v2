using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Tests;

public sealed class GoDaddyPersistenceModelTests
{
    public static IEnumerable<object[]> GoDaddyTables =>
    [
        [typeof(GoDaddyApiHealth), "GoDaddyApiHealth"],
        [typeof(GoDaddyChange), "GoDaddyChanges"],
        [typeof(GoDaddyDomainSnapshot), "GoDaddyDomainSnapshots"],
        [typeof(GoDaddyIncident), "GoDaddyIncidents"],
        [typeof(GoDaddyMaintenance), "GoDaddyMaintenance"],
        [typeof(GoDaddyRestApiEvidence), "GoDaddyRestApiEvidence"],
        [typeof(GoDaddyStatusComponent), "GoDaddyStatusComponents"],
        [typeof(GoDaddyStatusSnapshot), "GoDaddyStatusSnapshots"],
        [typeof(GoDaddyTransaction), "GoDaddyTransactions"]
    ];

    [Theory]
    [MemberData(nameof(GoDaddyTables))]
    public void EveryGoDaddyDatasetHasAnExplicitTable(Type entityType, string tableName)
    {
        var options = new DbContextOptionsBuilder<WipDbContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True").Options;
        using var context = new WipDbContext(options);
        var entity = context.Model.FindEntityType(entityType)!;
        Assert.Equal(tableName, entity.GetTableName());
        Assert.Contains(entity.GetIndexes(), index => index.IsUnique);
    }
}
