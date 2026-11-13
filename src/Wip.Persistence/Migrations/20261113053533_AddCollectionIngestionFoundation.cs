using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCollectionIngestionFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CollectorKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RequestedFromUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkflowId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExecutionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CompletenessStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngestionIdempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dataset = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PayloadHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FirstSeenAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionIdempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectionSourceAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    IsRetryable = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCategory = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PayloadHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextAttemptAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionSourceAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionSourceAttempts_CollectionRuns_CollectionRunId",
                        column: x => x.CollectionRunId,
                        principalTable: "CollectionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngestionErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceAttemptId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsRetryable = table.Column<bool>(type: "bit", nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextAttemptAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngestionErrors_CollectionRuns_CollectionRunId",
                        column: x => x.CollectionRunId,
                        principalTable: "CollectionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngestionErrors_CollectionSourceAttempts_SourceAttemptId",
                        column: x => x.SourceAttemptId,
                        principalTable: "CollectionSourceAttempts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RawProviderPayloads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceAttemptId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dataset = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PayloadHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RetentionUntilUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawProviderPayloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawProviderPayloads_CollectionRuns_CollectionRunId",
                        column: x => x.CollectionRunId,
                        principalTable: "CollectionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RawProviderPayloads_CollectionSourceAttempts_SourceAttemptId",
                        column: x => x.SourceAttemptId,
                        principalTable: "CollectionSourceAttempts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionRuns_Provider_CollectorKey_StartedAtUtc",
                table: "CollectionRuns",
                columns: new[] { "Provider", "CollectorKey", "StartedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionRuns_WorkflowId_ExecutionId",
                table: "CollectionRuns",
                columns: new[] { "WorkflowId", "ExecutionId" },
                unique: true,
                filter: "[WorkflowId] IS NOT NULL AND [ExecutionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionSourceAttempts_CollectionRunId_SourceKey_AttemptNumber",
                table: "CollectionSourceAttempts",
                columns: new[] { "CollectionRunId", "SourceKey", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionSourceAttempts_CollectionRunId_Status",
                table: "CollectionSourceAttempts",
                columns: new[] { "CollectionRunId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_IngestionErrors_CollectionRunId_OccurredAtUtc",
                table: "IngestionErrors",
                columns: new[] { "CollectionRunId", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_IngestionErrors_IsRetryable_NextAttemptAtUtc",
                table: "IngestionErrors",
                columns: new[] { "IsRetryable", "NextAttemptAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_IngestionErrors_SourceAttemptId",
                table: "IngestionErrors",
                column: "SourceAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_IngestionIdempotency_Provider_Dataset_IdempotencyKey",
                table: "IngestionIdempotency",
                columns: new[] { "Provider", "Dataset", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RawProviderPayloads_CollectionRunId",
                table: "RawProviderPayloads",
                column: "CollectionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_RawProviderPayloads_Provider_Dataset_PayloadHash",
                table: "RawProviderPayloads",
                columns: new[] { "Provider", "Dataset", "PayloadHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RawProviderPayloads_SourceAttemptId",
                table: "RawProviderPayloads",
                column: "SourceAttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngestionErrors");

            migrationBuilder.DropTable(
                name: "IngestionIdempotency");

            migrationBuilder.DropTable(
                name: "RawProviderPayloads");

            migrationBuilder.DropTable(
                name: "CollectionSourceAttempts");

            migrationBuilder.DropTable(
                name: "CollectionRuns");
        }
    }
}
