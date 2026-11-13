using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCloudflareDataSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudflareAccountSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    WorkflowRunId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareAccountSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareAttentionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareAttentionItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareCacheDaily",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    CacheStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareCacheDaily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareCacheIntervals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IntervalStartUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntervalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareCacheIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareConfigurationChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareConfigurationChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareDailyReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    WorkflowRunId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DataQualityStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    TotalRequests = table.Column<long>(type: "bigint", nullable: true),
                    TotalBandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    TotalHttpErrors = table.Column<long>(type: "bigint", nullable: true),
                    ErrorRatePercentage = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareDailyReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareDnssecSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DnssecStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareDnssecSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareDnsSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareDnsSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareHttpErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IntervalStartUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntervalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareHttpErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareHttpErrorsDaily",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareHttpErrorsDaily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareOriginHourly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateTimeHourUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    AvgOriginResponseMs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareOriginHourly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareOriginIntervals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IntervalStartUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntervalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    AvgOriginResponseMs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareOriginIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflarePerformanceConfigSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ConfigurationFingerprint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflarePerformanceConfigSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareRuleSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    RuleCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareRuleSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareSecurityConfigSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ConfigurationFingerprint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareSecurityConfigSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareSecurityDaily",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareSecurityDaily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareSslTlsSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    SslMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareSslTlsSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareTrafficHourly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateTimeHourUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    Visits = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareTrafficHourly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareTrafficIntervals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    IntervalStartUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntervalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requests = table.Column<long>(type: "bigint", nullable: true),
                    Visits = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareTrafficIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudflareZoneSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZoneName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDateKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ZoneStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudflareZoneSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareAccountSnapshots_RecordKey",
                table: "CloudflareAccountSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareAccountSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareAccountSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareAttentionItems_RecordKey",
                table: "CloudflareAttentionItems",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareAttentionItems_ZoneId_ReportDateKey",
                table: "CloudflareAttentionItems",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareCacheDaily_RecordKey",
                table: "CloudflareCacheDaily",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareCacheDaily_ZoneId_ReportDateKey",
                table: "CloudflareCacheDaily",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareCacheIntervals_RecordKey",
                table: "CloudflareCacheIntervals",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareCacheIntervals_ZoneId_ReportDateKey",
                table: "CloudflareCacheIntervals",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareConfigurationChanges_RecordKey",
                table: "CloudflareConfigurationChanges",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareConfigurationChanges_ZoneId_ReportDateKey",
                table: "CloudflareConfigurationChanges",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDailyReports_RecordKey",
                table: "CloudflareDailyReports",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDailyReports_ZoneId_ReportDateKey",
                table: "CloudflareDailyReports",
                columns: new[] { "ZoneId", "ReportDateKey" },
                unique: true,
                filter: "[ZoneId] IS NOT NULL AND [ReportDateKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDnssecSnapshots_RecordKey",
                table: "CloudflareDnssecSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDnssecSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareDnssecSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDnsSnapshots_RecordKey",
                table: "CloudflareDnsSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareDnsSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareDnsSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareHttpErrors_RecordKey",
                table: "CloudflareHttpErrors",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareHttpErrors_ZoneId_ReportDateKey",
                table: "CloudflareHttpErrors",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareHttpErrorsDaily_RecordKey",
                table: "CloudflareHttpErrorsDaily",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareHttpErrorsDaily_ZoneId_ReportDateKey",
                table: "CloudflareHttpErrorsDaily",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareOriginHourly_RecordKey",
                table: "CloudflareOriginHourly",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareOriginHourly_ZoneId_ReportDateKey",
                table: "CloudflareOriginHourly",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareOriginIntervals_RecordKey",
                table: "CloudflareOriginIntervals",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareOriginIntervals_ZoneId_ReportDateKey",
                table: "CloudflareOriginIntervals",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflarePerformanceConfigSnapshots_RecordKey",
                table: "CloudflarePerformanceConfigSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflarePerformanceConfigSnapshots_ZoneId_ReportDateKey",
                table: "CloudflarePerformanceConfigSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareRuleSnapshots_RecordKey",
                table: "CloudflareRuleSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareRuleSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareRuleSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSecurityConfigSnapshots_RecordKey",
                table: "CloudflareSecurityConfigSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSecurityConfigSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareSecurityConfigSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSecurityDaily_RecordKey",
                table: "CloudflareSecurityDaily",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSecurityDaily_ZoneId_ReportDateKey",
                table: "CloudflareSecurityDaily",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSslTlsSnapshots_RecordKey",
                table: "CloudflareSslTlsSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareSslTlsSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareSslTlsSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareTrafficHourly_RecordKey",
                table: "CloudflareTrafficHourly",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareTrafficHourly_ZoneId_ReportDateKey",
                table: "CloudflareTrafficHourly",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareTrafficIntervals_RecordKey",
                table: "CloudflareTrafficIntervals",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareTrafficIntervals_ZoneId_ReportDateKey",
                table: "CloudflareTrafficIntervals",
                columns: new[] { "ZoneId", "ReportDateKey" });

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareZoneSnapshots_RecordKey",
                table: "CloudflareZoneSnapshots",
                column: "RecordKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudflareZoneSnapshots_ZoneId_ReportDateKey",
                table: "CloudflareZoneSnapshots",
                columns: new[] { "ZoneId", "ReportDateKey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudflareAccountSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareAttentionItems");

            migrationBuilder.DropTable(
                name: "CloudflareCacheDaily");

            migrationBuilder.DropTable(
                name: "CloudflareCacheIntervals");

            migrationBuilder.DropTable(
                name: "CloudflareConfigurationChanges");

            migrationBuilder.DropTable(
                name: "CloudflareDailyReports");

            migrationBuilder.DropTable(
                name: "CloudflareDnssecSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareDnsSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareHttpErrors");

            migrationBuilder.DropTable(
                name: "CloudflareHttpErrorsDaily");

            migrationBuilder.DropTable(
                name: "CloudflareOriginHourly");

            migrationBuilder.DropTable(
                name: "CloudflareOriginIntervals");

            migrationBuilder.DropTable(
                name: "CloudflarePerformanceConfigSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareRuleSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareSecurityConfigSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareSecurityDaily");

            migrationBuilder.DropTable(
                name: "CloudflareSslTlsSnapshots");

            migrationBuilder.DropTable(
                name: "CloudflareTrafficHourly");

            migrationBuilder.DropTable(
                name: "CloudflareTrafficIntervals");

            migrationBuilder.DropTable(
                name: "CloudflareZoneSnapshots");
        }
    }
}
