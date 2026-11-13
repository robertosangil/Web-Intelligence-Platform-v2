namespace Wip.Persistence;

public static class ModuleCatalog
{
    public static readonly IReadOnlyList<Module> All =
    [
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Key = "dashboard_overview", Name = "Overview", GroupKey = "dashboard", SortOrder = 1 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Key = "dashboard_executive_summary", Name = "Executive Summary", GroupKey = "dashboard", SortOrder = 2 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Key = "dashboard_attention_issues", Name = "Attention / Issues", GroupKey = "dashboard", SortOrder = 3 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Key = "dashboard_reports", Name = "Reports", GroupKey = "dashboard", SortOrder = 4 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000101"), Key = "cloudflare_overview", Name = "Overview", GroupKey = "cloudflare", SortOrder = 1 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000102"), Key = "cloudflare_trafficbandwidth", Name = "Traffic & Bandwidth", GroupKey = "cloudflare", SortOrder = 2 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000103"), Key = "cloudflare_cache", Name = "Cache Performance", GroupKey = "cloudflare", SortOrder = 3 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000104"), Key = "cloudflare_origin", Name = "Origin Performance", GroupKey = "cloudflare", SortOrder = 4 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000105"), Key = "cloudflare_http_errors", Name = "HTTP Errors", GroupKey = "cloudflare", SortOrder = 5 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000106"), Key = "cloudflare_security", Name = "Security", GroupKey = "cloudflare", SortOrder = 6 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000107"), Key = "cloudflare_configuration_changes", Name = "Configuration & Changes", GroupKey = "cloudflare", SortOrder = 7 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000108"), Key = "cloudflare_data_quality", Name = "Data Quality", GroupKey = "cloudflare", SortOrder = 8 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000201"), Key = "godaddy_overview", Name = "Overview", GroupKey = "godaddy", SortOrder = 1 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000202"), Key = "godaddy_domain_status", Name = "Domain Status", GroupKey = "godaddy", SortOrder = 2 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000203"), Key = "godaddy_dns", Name = "DNS", GroupKey = "godaddy", SortOrder = 3 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000204"), Key = "godaddy_domain_changes", Name = "Domain Changes", GroupKey = "godaddy", SortOrder = 4 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000205"), Key = "godaddy_risk_alerts", Name = "Risk / Alerts", GroupKey = "godaddy", SortOrder = 5 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000301"), Key = "cpanel_overview", Name = "Overview", GroupKey = "cpanel", SortOrder = 1 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000302"), Key = "cpanel_resources", Name = "Resources", GroupKey = "cpanel", SortOrder = 2 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000303"), Key = "cpanel_daily_statistics", Name = "Daily Statistics", GroupKey = "cpanel", SortOrder = 3 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000304"), Key = "cpanel_server_errors", Name = "Server Errors", GroupKey = "cpanel", SortOrder = 4 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000305"), Key = "cpanel_ssl_autossl", Name = "SSL / AutoSSL", GroupKey = "cpanel", SortOrder = 5 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000306"), Key = "cpanel_backups", Name = "Backups", GroupKey = "cpanel", SortOrder = 6 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000401"), Key = "wp_umbrella_overview", Name = "Overview", GroupKey = "wp_umbrella", SortOrder = 1 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000402"), Key = "wp_umbrella_site_health", Name = "Site Health", GroupKey = "wp_umbrella", SortOrder = 2 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000403"), Key = "wp_umbrella_plugins", Name = "Plugins", GroupKey = "wp_umbrella", SortOrder = 3 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000404"), Key = "wp_umbrella_themes", Name = "Themes", GroupKey = "wp_umbrella", SortOrder = 4 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000405"), Key = "wp_umbrella_vulnerabilities", Name = "Vulnerabilities", GroupKey = "wp_umbrella", SortOrder = 5 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000406"), Key = "wp_umbrella_uptime", Name = "Uptime", GroupKey = "wp_umbrella", SortOrder = 6 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000407"), Key = "wp_umbrella_incidents", Name = "Incidents", GroupKey = "wp_umbrella", SortOrder = 7 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000408"), Key = "wp_umbrella_backups", Name = "Backups", GroupKey = "wp_umbrella", SortOrder = 8 },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000501"), Key = "settings_user_management", Name = "User Management", GroupKey = "settings", SortOrder = 1 }
    ];
}
