using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGoDaddyDataSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoDaddyApiHealth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpStatus = table.Column<int>(type: "int", nullable: true),
                    ConsecutiveFailures = table.Column<int>(type: "int", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyApiHealth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyDomainSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RenewByUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: true),
                    RawEvidenceKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyDomainSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProviderIncidentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrentOpen = table.Column<bool>(type: "bit", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyIncidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyMaintenance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProviderMaintenanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledForUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledUntilUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyMaintenance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyRestApiEvidence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    RecordCount = table.Column<int>(type: "int", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyRestApiEvidence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyStatusComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentSnapshotKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyStatusComponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyStatusSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveIncidentCount = table.Column<int>(type: "int", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyStatusSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoDaddyTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TransactionKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RunType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowRunId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceCount = table.Column<int>(type: "int", nullable: true),
                    SuccessCount = table.Column<int>(type: "int", nullable: true),
                    FailedCount = table.Column<int>(type: "int", nullable: true),
                    DataCompletenessStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportReady = table.Column<bool>(type: "bit", nullable: true),
                    PendingSourcesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MissingSourcesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoDaddyTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyApiHealth_HealthKey",
                table: "GoDaddyApiHealth",
                column: "HealthKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyChanges_ChangeKey",
                table: "GoDaddyChanges",
                column: "ChangeKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyDomainSnapshots_DomainKey",
                table: "GoDaddyDomainSnapshots",
                column: "DomainKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyIncidents_IncidentKey",
                table: "GoDaddyIncidents",
                column: "IncidentKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyMaintenance_MaintenanceKey",
                table: "GoDaddyMaintenance",
                column: "MaintenanceKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyRestApiEvidence_ApiKey",
                table: "GoDaddyRestApiEvidence",
                column: "ApiKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyStatusComponents_ComponentSnapshotKey",
                table: "GoDaddyStatusComponents",
                column: "ComponentSnapshotKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyStatusSnapshots_StatusKey",
                table: "GoDaddyStatusSnapshots",
                column: "StatusKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoDaddyTransactions_TransactionId",
                table: "GoDaddyTransactions",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoDaddyApiHealth");

            migrationBuilder.DropTable(
                name: "GoDaddyChanges");

            migrationBuilder.DropTable(
                name: "GoDaddyDomainSnapshots");

            migrationBuilder.DropTable(
                name: "GoDaddyIncidents");

            migrationBuilder.DropTable(
                name: "GoDaddyMaintenance");

            migrationBuilder.DropTable(
                name: "GoDaddyRestApiEvidence");

            migrationBuilder.DropTable(
                name: "GoDaddyStatusComponents");

            migrationBuilder.DropTable(
                name: "GoDaddyStatusSnapshots");

            migrationBuilder.DropTable(
                name: "GoDaddyTransactions");
        }
    }
}
