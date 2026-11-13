using Microsoft.EntityFrameworkCore;
using Wip.Persistence;

namespace Wip.Api.Tests;

public sealed class CloudflarePersistenceModelTests
{
    public static IEnumerable<object[]> CloudflareTables =>
    [
        [typeof(CloudflareAccountSnapshot), "CloudflareAccountSnapshots"],
        [typeof(CloudflareAttentionItem), "CloudflareAttentionItems"],
        [typeof(CloudflareCacheInterval), "CloudflareCacheIntervals"],
        [typeof(CloudflareCacheDaily), "CloudflareCacheDaily"],
        [typeof(CloudflareConfigurationChange), "CloudflareConfigurationChanges"],
        [typeof(CloudflareDailyReport), "CloudflareDailyReports"],
        [typeof(CloudflareDnsSnapshot), "CloudflareDnsSnapshots"],
        [typeof(CloudflareDnssecSnapshot), "CloudflareDnssecSnapshots"],
        [typeof(CloudflareHttpError), "CloudflareHttpErrors"],
        [typeof(CloudflareHttpErrorDaily), "CloudflareHttpErrorsDaily"],
        [typeof(CloudflareOriginInterval), "CloudflareOriginIntervals"],
        [typeof(CloudflareOriginHourly), "CloudflareOriginHourly"],
        [typeof(CloudflarePerformanceConfigSnapshot), "CloudflarePerformanceConfigSnapshots"],
        [typeof(CloudflareRuleSnapshot), "CloudflareRuleSnapshots"],
        [typeof(CloudflareSecurityConfigSnapshot), "CloudflareSecurityConfigSnapshots"],
        [typeof(CloudflareSecurityDaily), "CloudflareSecurityDaily"],
        [typeof(CloudflareSslTlsSnapshot), "CloudflareSslTlsSnapshots"],
        [typeof(CloudflareTrafficInterval), "CloudflareTrafficIntervals"],
        [typeof(CloudflareTrafficHourly), "CloudflareTrafficHourly"],
        [typeof(CloudflareZoneSnapshot), "CloudflareZoneSnapshots"]
    ];

    [Theory]
    [MemberData(nameof(CloudflareTables))]
    public void EveryCloudflareDatasetHasAnExplicitTable(Type entityType, string tableName)
    {
        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True")
            .Options;

        using var context = new WipDbContext(options);
        var entity = context.Model.FindEntityType(entityType)!;

        Assert.Equal(tableName, entity.GetTableName());
        Assert.Contains(entity.GetIndexes(), index => index.IsUnique);
    }

    [Fact]
    public void CloudflareDailyReportPreservesCoreMetricsAndPayload()
    {
        var options = new DbContextOptionsBuilder<WipDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelOnly;Trusted_Connection=True")
            .Options;

        using var context = new WipDbContext(options);
        var entity = context.Model.FindEntityType(typeof(CloudflareDailyReport))!;

        Assert.NotNull(entity.FindProperty(nameof(CloudflareDailyReport.TotalRequests)));
        Assert.NotNull(entity.FindProperty(nameof(CloudflareDailyReport.TotalBandwidthBytes)));
        Assert.NotNull(entity.FindProperty(nameof(CloudflareDailyReport.DataQualityStatus)));
        Assert.NotNull(entity.FindProperty(nameof(CloudflareDailyReport.PayloadJson)));
    }
}
