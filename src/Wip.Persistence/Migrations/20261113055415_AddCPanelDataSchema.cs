using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCPanelDataSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CPanelApiEvidence",
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
                    table.PrimaryKey("PK_CPanelApiEvidence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelAutoSsl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoSslKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelAutoSsl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelBackups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackupKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelBackups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelCapabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CapabilityKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelCapabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelDailyStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DailyStatKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatDateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpuSeconds = table.Column<long>(type: "bigint", nullable: true),
                    DiskBytes = table.Column<long>(type: "bigint", nullable: true),
                    BandwidthBytes = table.Column<long>(type: "bigint", nullable: true),
                    EmailAccounts = table.Column<int>(type: "int", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelDailyStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelDatabases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatabaseKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatabaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelDatabases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelResourceDailySummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummaryKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryDateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LimitValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelResourceDailySummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LimitValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelResourceSamplesLegacy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SampleKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampledAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelResourceSamplesLegacy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelServerErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ErrorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccurredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelServerErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelServerInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPanelVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhpVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelServerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelSslCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelSslCertificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPanelTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceCount = table.Column<int>(type: "int", nullable: true),
                    SuccessCount = table.Column<int>(type: "int", nullable: true),
                    FailedCount = table.Column<int>(type: "int", nullable: true),
                    DataCompletenessStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPanelTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPanelApiEvidence_EvidenceKey",
                table: "CPanelApiEvidence",
                column: "EvidenceKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelAutoSsl_AutoSslKey",
                table: "CPanelAutoSsl",
                column: "AutoSslKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelBackups_BackupKey",
                table: "CPanelBackups",
                column: "BackupKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelCapabilities_CapabilityKey",
                table: "CPanelCapabilities",
                column: "CapabilityKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelDailyStats_DailyStatKey",
                table: "CPanelDailyStats",
                column: "DailyStatKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelDatabases_DatabaseKey",
                table: "CPanelDatabases",
                column: "DatabaseKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelResourceDailySummaries_SummaryKey",
                table: "CPanelResourceDailySummaries",
                column: "SummaryKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelResources_ResourceKey",
                table: "CPanelResources",
                column: "ResourceKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelResourceSamplesLegacy_SampleKey",
                table: "CPanelResourceSamplesLegacy",
                column: "SampleKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelServerErrors_ErrorKey",
                table: "CPanelServerErrors",
                column: "ErrorKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelServerInfo_ServerKey",
                table: "CPanelServerInfo",
                column: "ServerKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelSslCertificates_CertificateKey",
                table: "CPanelSslCertificates",
                column: "CertificateKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPanelTransactions_TransactionId",
                table: "CPanelTransactions",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPanelApiEvidence");

            migrationBuilder.DropTable(
                name: "CPanelAutoSsl");

            migrationBuilder.DropTable(
                name: "CPanelBackups");

            migrationBuilder.DropTable(
                name: "CPanelCapabilities");

            migrationBuilder.DropTable(
                name: "CPanelDailyStats");

            migrationBuilder.DropTable(
                name: "CPanelDatabases");

            migrationBuilder.DropTable(
                name: "CPanelResourceDailySummaries");

            migrationBuilder.DropTable(
                name: "CPanelResources");

            migrationBuilder.DropTable(
                name: "CPanelResourceSamplesLegacy");

            migrationBuilder.DropTable(
                name: "CPanelServerErrors");

            migrationBuilder.DropTable(
                name: "CPanelServerInfo");

            migrationBuilder.DropTable(
                name: "CPanelSslCertificates");

            migrationBuilder.DropTable(
                name: "CPanelTransactions");
        }
    }
}
