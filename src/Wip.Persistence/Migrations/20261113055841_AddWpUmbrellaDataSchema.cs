using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWpUmbrellaDataSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WpUmbrellaApiEvidence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvidenceKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    CollectionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordCount = table.Column<int>(type: "int", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaApiEvidence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaAttentionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttentionKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaAttentionItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaBackups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackupKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaBackups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaDailySummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummaryKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryDateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PluginCount = table.Column<int>(type: "int", nullable: true),
                    ThemeCount = table.Column<int>(type: "int", nullable: true),
                    VulnerabilityCount = table.Column<int>(type: "int", nullable: true),
                    UptimePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaDailySummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaPhpInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhpInfoKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhpVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemoryLimit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxExecutionTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaPhpInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaPhpIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaPhpIssues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaPlugins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PluginKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PluginName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdateAvailable = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaPlugins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaProjectSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaProjectSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaThemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThemeKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdateAvailable = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaThemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaUptime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UptimeKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodDateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UptimePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckCount = table.Column<int>(type: "int", nullable: true),
                    FailureCount = table.Column<int>(type: "int", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaUptime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaUptimeIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaUptimeIncidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WpUmbrellaVulnerabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VulnerabilityKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VulnerabilityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WpUmbrellaVulnerabilities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaApiEvidence_EvidenceKey",
                table: "WpUmbrellaApiEvidence",
                column: "EvidenceKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaAttentionItems_AttentionKey",
                table: "WpUmbrellaAttentionItems",
                column: "AttentionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaBackups_BackupKey",
                table: "WpUmbrellaBackups",
                column: "BackupKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaChanges_ChangeKey",
                table: "WpUmbrellaChanges",
                column: "ChangeKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaDailySummaries_SummaryKey",
                table: "WpUmbrellaDailySummaries",
                column: "SummaryKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaPhpInfo_PhpInfoKey",
                table: "WpUmbrellaPhpInfo",
                column: "PhpInfoKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaPhpIssues_IssueKey",
                table: "WpUmbrellaPhpIssues",
                column: "IssueKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaPlugins_PluginKey",
                table: "WpUmbrellaPlugins",
                column: "PluginKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaProcesses_ProcessKey",
                table: "WpUmbrellaProcesses",
                column: "ProcessKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaProjectSnapshots_ProjectKey",
                table: "WpUmbrellaProjectSnapshots",
                column: "ProjectKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaThemes_ThemeKey",
                table: "WpUmbrellaThemes",
                column: "ThemeKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaUptime_UptimeKey",
                table: "WpUmbrellaUptime",
                column: "UptimeKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaUptimeIncidents_IncidentKey",
                table: "WpUmbrellaUptimeIncidents",
                column: "IncidentKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WpUmbrellaVulnerabilities_VulnerabilityKey",
                table: "WpUmbrellaVulnerabilities",
                column: "VulnerabilityKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WpUmbrellaApiEvidence");

            migrationBuilder.DropTable(
                name: "WpUmbrellaAttentionItems");

            migrationBuilder.DropTable(
                name: "WpUmbrellaBackups");

            migrationBuilder.DropTable(
                name: "WpUmbrellaChanges");

            migrationBuilder.DropTable(
                name: "WpUmbrellaDailySummaries");

            migrationBuilder.DropTable(
                name: "WpUmbrellaPhpInfo");

            migrationBuilder.DropTable(
                name: "WpUmbrellaPhpIssues");

            migrationBuilder.DropTable(
                name: "WpUmbrellaPlugins");

            migrationBuilder.DropTable(
                name: "WpUmbrellaProcesses");

            migrationBuilder.DropTable(
                name: "WpUmbrellaProjectSnapshots");

            migrationBuilder.DropTable(
                name: "WpUmbrellaThemes");

            migrationBuilder.DropTable(
                name: "WpUmbrellaUptime");

            migrationBuilder.DropTable(
                name: "WpUmbrellaUptimeIncidents");

            migrationBuilder.DropTable(
                name: "WpUmbrellaVulnerabilities");
        }
    }
}
