using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Tests;

public sealed class CollectionPersistenceModelTests
{
    [Fact]
    public void CollectionRunModelUsesRequiredOperationalTablesAndKeys()
    {
        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True")
            .Options;

        using var context = new WipDbContext(options);
        var model = context.Model;

        Assert.Equal("CollectionRuns", model.FindEntityType(typeof(CollectionRun))!.GetTableName());
        Assert.Equal("CollectionSourceAttempts", model.FindEntityType(typeof(CollectionSourceAttempt))!.GetTableName());
        Assert.Equal("RawProviderPayloads", model.FindEntityType(typeof(RawProviderPayload))!.GetTableName());
        Assert.Equal("IngestionIdempotency", model.FindEntityType(typeof(IngestionIdempotencyRecord))!.GetTableName());
        Assert.Equal("IngestionErrors", model.FindEntityType(typeof(IngestionError))!.GetTableName());
        Assert.Contains(model.FindEntityType(typeof(IngestionIdempotencyRecord))!.GetIndexes(), index => index.IsUnique);
        Assert.Contains(model.FindEntityType(typeof(CollectionSourceAttempt))!.GetIndexes(), index => index.IsUnique);
    }

    [Fact]
    public void OperationalRecordsHaveRunRelationships()
    {
        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True")
            .Options;

        using var context = new WipDbContext(options);
        var source = context.Model.FindEntityType(typeof(CollectionSourceAttempt))!;
        var payload = context.Model.FindEntityType(typeof(RawProviderPayload))!;
        var error = context.Model.FindEntityType(typeof(IngestionError))!;

        Assert.Contains(source.GetForeignKeys(), key => key.PrincipalEntityType.ClrType == typeof(CollectionRun));
        Assert.Contains(payload.GetForeignKeys(), key => key.PrincipalEntityType.ClrType == typeof(CollectionRun));
        Assert.Contains(error.GetForeignKeys(), key => key.PrincipalEntityType.ClrType == typeof(CollectionRun));
    }
}
