using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wip.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedFixedPageModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupKey",
                table: "Modules",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "GroupKey", "Key", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "dashboard", "dashboard_overview", "Overview", 1 },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "dashboard", "dashboard_executive_summary", "Executive Summary", 2 },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "dashboard", "dashboard_attention_issues", "Attention / Issues", 3 },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "dashboard", "dashboard_reports", "Reports", 4 },
                    { new Guid("10000000-0000-0000-0000-000000000101"), "cloudflare", "cloudflare_overview", "Overview", 1 },
                    { new Guid("10000000-0000-0000-0000-000000000102"), "cloudflare", "cloudflare_trafficbandwidth", "Traffic & Bandwidth", 2 },
                    { new Guid("10000000-0000-0000-0000-000000000103"), "cloudflare", "cloudflare_cache", "Cache Performance", 3 },
                    { new Guid("10000000-0000-0000-0000-000000000104"), "cloudflare", "cloudflare_origin", "Origin Performance", 4 },
                    { new Guid("10000000-0000-0000-0000-000000000105"), "cloudflare", "cloudflare_http_errors", "HTTP Errors", 5 },
                    { new Guid("10000000-0000-0000-0000-000000000106"), "cloudflare", "cloudflare_security", "Security", 6 },
                    { new Guid("10000000-0000-0000-0000-000000000107"), "cloudflare", "cloudflare_configuration_changes", "Configuration & Changes", 7 },
                    { new Guid("10000000-0000-0000-0000-000000000108"), "cloudflare", "cloudflare_data_quality", "Data Quality", 8 },
                    { new Guid("10000000-0000-0000-0000-000000000201"), "godaddy", "godaddy_overview", "Overview", 1 },
                    { new Guid("10000000-0000-0000-0000-000000000202"), "godaddy", "godaddy_domain_status", "Domain Status", 2 },
                    { new Guid("10000000-0000-0000-0000-000000000203"), "godaddy", "godaddy_dns", "DNS", 3 },
                    { new Guid("10000000-0000-0000-0000-000000000204"), "godaddy", "godaddy_domain_changes", "Domain Changes", 4 },
                    { new Guid("10000000-0000-0000-0000-000000000205"), "godaddy", "godaddy_risk_alerts", "Risk / Alerts", 5 },
                    { new Guid("10000000-0000-0000-0000-000000000301"), "cpanel", "cpanel_overview", "Overview", 1 },
                    { new Guid("10000000-0000-0000-0000-000000000302"), "cpanel", "cpanel_resources", "Resources", 2 },
                    { new Guid("10000000-0000-0000-0000-000000000303"), "cpanel", "cpanel_daily_statistics", "Daily Statistics", 3 },
                    { new Guid("10000000-0000-0000-0000-000000000304"), "cpanel", "cpanel_server_errors", "Server Errors", 4 },
                    { new Guid("10000000-0000-0000-0000-000000000305"), "cpanel", "cpanel_ssl_autossl", "SSL / AutoSSL", 5 },
                    { new Guid("10000000-0000-0000-0000-000000000306"), "cpanel", "cpanel_backups", "Backups", 6 },
                    { new Guid("10000000-0000-0000-0000-000000000401"), "wp_umbrella", "wp_umbrella_overview", "Overview", 1 },
                    { new Guid("10000000-0000-0000-0000-000000000402"), "wp_umbrella", "wp_umbrella_site_health", "Site Health", 2 },
                    { new Guid("10000000-0000-0000-0000-000000000403"), "wp_umbrella", "wp_umbrella_plugins", "Plugins", 3 },
                    { new Guid("10000000-0000-0000-0000-000000000404"), "wp_umbrella", "wp_umbrella_themes", "Themes", 4 },
                    { new Guid("10000000-0000-0000-0000-000000000405"), "wp_umbrella", "wp_umbrella_vulnerabilities", "Vulnerabilities", 5 },
                    { new Guid("10000000-0000-0000-0000-000000000406"), "wp_umbrella", "wp_umbrella_uptime", "Uptime", 6 },
                    { new Guid("10000000-0000-0000-0000-000000000407"), "wp_umbrella", "wp_umbrella_incidents", "Incidents", 7 },
                    { new Guid("10000000-0000-0000-0000-000000000408"), "wp_umbrella", "wp_umbrella_backups", "Backups", 8 },
                    { new Guid("10000000-0000-0000-0000-000000000501"), "settings", "settings_user_management", "User Management", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_GroupKey_SortOrder",
                table: "Modules",
                columns: new[] { "GroupKey", "SortOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Modules_GroupKey_SortOrder",
                table: "Modules");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000201"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000202"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000203"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000204"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000205"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000301"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000302"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000303"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000304"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000305"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000306"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000401"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000402"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000403"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000404"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000405"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000406"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000407"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000408"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000501"));

            migrationBuilder.DropColumn(
                name: "GroupKey",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Modules");
        }
    }
}
