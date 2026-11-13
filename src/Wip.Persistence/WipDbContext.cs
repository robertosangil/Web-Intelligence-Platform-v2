using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wip.Persistence.Identity;

namespace Wip.Persistence;

public sealed class WipDbContext(DbContextOptions<WipDbContext> options)
    : IdentityDbContext<WipUser, WipRole, Guid>(options)
{
    public DbSet<Module> Modules => Set<Module>();

    public DbSet<RoleModule> RoleModules => Set<RoleModule>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<CollectionRun> CollectionRuns => Set<CollectionRun>();
    public DbSet<CollectionSourceAttempt> CollectionSourceAttempts => Set<CollectionSourceAttempt>();
    public DbSet<RawProviderPayload> RawProviderPayloads => Set<RawProviderPayload>();
    public DbSet<IngestionIdempotencyRecord> IngestionIdempotency => Set<IngestionIdempotencyRecord>();
    public DbSet<IngestionError> IngestionErrors => Set<IngestionError>();
    public DbSet<CloudflareAccountSnapshot> CloudflareAccountSnapshots => Set<CloudflareAccountSnapshot>();
    public DbSet<CloudflareAttentionItem> CloudflareAttentionItems => Set<CloudflareAttentionItem>();
    public DbSet<CloudflareCacheInterval> CloudflareCacheIntervals => Set<CloudflareCacheInterval>();
    public DbSet<CloudflareCacheDaily> CloudflareCacheDaily => Set<CloudflareCacheDaily>();
    public DbSet<CloudflareConfigurationChange> CloudflareConfigurationChanges => Set<CloudflareConfigurationChange>();
    public DbSet<CloudflareDailyReport> CloudflareDailyReports => Set<CloudflareDailyReport>();
    public DbSet<CloudflareDnsSnapshot> CloudflareDnsSnapshots => Set<CloudflareDnsSnapshot>();
    public DbSet<CloudflareDnssecSnapshot> CloudflareDnssecSnapshots => Set<CloudflareDnssecSnapshot>();
    public DbSet<CloudflareHttpError> CloudflareHttpErrors => Set<CloudflareHttpError>();
    public DbSet<CloudflareHttpErrorDaily> CloudflareHttpErrorsDaily => Set<CloudflareHttpErrorDaily>();
    public DbSet<CloudflareOriginInterval> CloudflareOriginIntervals => Set<CloudflareOriginInterval>();
    public DbSet<CloudflareOriginHourly> CloudflareOriginHourly => Set<CloudflareOriginHourly>();
    public DbSet<CloudflarePerformanceConfigSnapshot> CloudflarePerformanceConfigSnapshots => Set<CloudflarePerformanceConfigSnapshot>();
    public DbSet<CloudflareRuleSnapshot> CloudflareRuleSnapshots => Set<CloudflareRuleSnapshot>();
    public DbSet<CloudflareSecurityConfigSnapshot> CloudflareSecurityConfigSnapshots => Set<CloudflareSecurityConfigSnapshot>();
    public DbSet<CloudflareSecurityDaily> CloudflareSecurityDaily => Set<CloudflareSecurityDaily>();
    public DbSet<CloudflareSslTlsSnapshot> CloudflareSslTlsSnapshots => Set<CloudflareSslTlsSnapshot>();
    public DbSet<CloudflareTrafficInterval> CloudflareTrafficIntervals => Set<CloudflareTrafficInterval>();
    public DbSet<CloudflareTrafficHourly> CloudflareTrafficHourly => Set<CloudflareTrafficHourly>();
    public DbSet<CloudflareZoneSnapshot> CloudflareZoneSnapshots => Set<CloudflareZoneSnapshot>();
    public DbSet<GoDaddyApiHealth> GoDaddyApiHealth => Set<GoDaddyApiHealth>();
    public DbSet<GoDaddyChange> GoDaddyChanges => Set<GoDaddyChange>();
    public DbSet<GoDaddyDomainSnapshot> GoDaddyDomainSnapshots => Set<GoDaddyDomainSnapshot>();
    public DbSet<GoDaddyIncident> GoDaddyIncidents => Set<GoDaddyIncident>();
    public DbSet<GoDaddyMaintenance> GoDaddyMaintenance => Set<GoDaddyMaintenance>();
    public DbSet<GoDaddyRestApiEvidence> GoDaddyRestApiEvidence => Set<GoDaddyRestApiEvidence>();
    public DbSet<GoDaddyStatusComponent> GoDaddyStatusComponents => Set<GoDaddyStatusComponent>();
    public DbSet<GoDaddyStatusSnapshot> GoDaddyStatusSnapshots => Set<GoDaddyStatusSnapshot>();
    public DbSet<GoDaddyTransaction> GoDaddyTransactions => Set<GoDaddyTransaction>();
    public DbSet<CPanelApiEvidence> CPanelApiEvidence => Set<CPanelApiEvidence>();
    public DbSet<CPanelAutoSsl> CPanelAutoSsl => Set<CPanelAutoSsl>();
    public DbSet<CPanelBackup> CPanelBackups => Set<CPanelBackup>();
    public DbSet<CPanelCapability> CPanelCapabilities => Set<CPanelCapability>();
    public DbSet<CPanelDailyStat> CPanelDailyStats => Set<CPanelDailyStat>();
    public DbSet<CPanelDatabase> CPanelDatabases => Set<CPanelDatabase>();
    public DbSet<CPanelResourceDailySummary> CPanelResourceDailySummaries => Set<CPanelResourceDailySummary>();
    public DbSet<CPanelResourceSampleLegacy> CPanelResourceSamplesLegacy => Set<CPanelResourceSampleLegacy>();
    public DbSet<CPanelResource> CPanelResources => Set<CPanelResource>();
    public DbSet<CPanelServerError> CPanelServerErrors => Set<CPanelServerError>();
    public DbSet<CPanelServerInfo> CPanelServerInfo => Set<CPanelServerInfo>();
    public DbSet<CPanelSslCertificate> CPanelSslCertificates => Set<CPanelSslCertificate>();
    public DbSet<CPanelTransaction> CPanelTransactions => Set<CPanelTransaction>();
    public DbSet<WpUmbrellaApiEvidence> WpUmbrellaApiEvidence => Set<WpUmbrellaApiEvidence>();
    public DbSet<WpUmbrellaAttentionItem> WpUmbrellaAttentionItems => Set<WpUmbrellaAttentionItem>();
    public DbSet<WpUmbrellaBackup> WpUmbrellaBackups => Set<WpUmbrellaBackup>();
    public DbSet<WpUmbrellaChange> WpUmbrellaChanges => Set<WpUmbrellaChange>();
    public DbSet<WpUmbrellaDailySummary> WpUmbrellaDailySummaries => Set<WpUmbrellaDailySummary>();
    public DbSet<WpUmbrellaPhpInfo> WpUmbrellaPhpInfo => Set<WpUmbrellaPhpInfo>();
    public DbSet<WpUmbrellaPhpIssue> WpUmbrellaPhpIssues => Set<WpUmbrellaPhpIssue>();
    public DbSet<WpUmbrellaPlugin> WpUmbrellaPlugins => Set<WpUmbrellaPlugin>();
    public DbSet<WpUmbrellaProcess> WpUmbrellaProcesses => Set<WpUmbrellaProcess>();
    public DbSet<WpUmbrellaProjectSnapshot> WpUmbrellaProjectSnapshots => Set<WpUmbrellaProjectSnapshot>();
    public DbSet<WpUmbrellaTheme> WpUmbrellaThemes => Set<WpUmbrellaTheme>();
    public DbSet<WpUmbrellaUptime> WpUmbrellaUptime => Set<WpUmbrellaUptime>();
    public DbSet<WpUmbrellaUptimeIncident> WpUmbrellaUptimeIncidents => Set<WpUmbrellaUptimeIncident>();
    public DbSet<WpUmbrellaVulnerability> WpUmbrellaVulnerabilities => Set<WpUmbrellaVulnerability>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WipUser>().ToTable("Users");
        builder.Entity<WipRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

        builder.Entity<Module>(entity =>
        {
            entity.ToTable("Modules");
            entity.HasKey(module => module.Id);
            entity.Property(module => module.Key).HasMaxLength(100).IsRequired();
            entity.Property(module => module.Name).HasMaxLength(200).IsRequired();
            entity.Property(module => module.GroupKey).HasMaxLength(100).IsRequired();
            entity.HasIndex(module => new { module.GroupKey, module.SortOrder }).IsUnique();
            entity.HasIndex(module => module.Key).IsUnique();
            entity.HasData(ModuleCatalog.All);
        });

        builder.Entity<RoleModule>(entity =>
        {
            entity.ToTable("RoleModules");
            entity.HasKey(roleModule => new { roleModule.RoleId, roleModule.ModuleId });
            entity.HasOne<WipRole>().WithMany().HasForeignKey(roleModule => roleModule.RoleId);
            entity.HasOne<Module>().WithMany().HasForeignKey(roleModule => roleModule.ModuleId);
        });
        builder.Entity<RefreshSession>(entity =>
        {
            entity.ToTable("RefreshSessions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasIndex(x => new { x.UserId, x.ExpiresAtUtc });
            entity.HasOne<WipUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetTokens");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Purpose).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasIndex(x => new { x.UserId, x.Purpose, x.ExpiresAtUtc });
            entity.HasOne<WipUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<CollectionRun>(entity =>
        {
            entity.ToTable("CollectionRuns");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CollectorKey).HasMaxLength(150).IsRequired();
            entity.Property(x => x.WorkflowId).HasMaxLength(200);
            entity.Property(x => x.ExecutionId).HasMaxLength(200);
            entity.Property(x => x.Status).HasMaxLength(40).IsRequired();
            entity.Property(x => x.CompletenessStatus).HasMaxLength(40);
            entity.HasIndex(x => new { x.Provider, x.CollectorKey, x.StartedAtUtc });
            entity.HasIndex(x => new { x.WorkflowId, x.ExecutionId }).IsUnique();
        });

        builder.Entity<CollectionSourceAttempt>(entity =>
        {
            entity.ToTable("CollectionSourceAttempts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SourceKey).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Endpoint).HasMaxLength(500);
            entity.Property(x => x.Status).HasMaxLength(40).IsRequired();
            entity.Property(x => x.ErrorCategory).HasMaxLength(80);
            entity.Property(x => x.ErrorMessage).HasMaxLength(2000);
            entity.Property(x => x.PayloadHash).HasMaxLength(128);
            entity.HasIndex(x => new { x.CollectionRunId, x.SourceKey, x.AttemptNumber }).IsUnique();
            entity.HasIndex(x => new { x.CollectionRunId, x.Status });
            entity.HasOne<CollectionRun>().WithMany().HasForeignKey(x => x.CollectionRunId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RawProviderPayload>(entity =>
        {
            entity.ToTable("RawProviderPayloads");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Dataset).HasMaxLength(150).IsRequired();
            entity.Property(x => x.PayloadHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.PayloadJson).IsRequired();
            entity.HasIndex(x => new { x.Provider, x.Dataset, x.PayloadHash }).IsUnique();
            entity.HasIndex(x => x.CollectionRunId);
            entity.HasOne<CollectionRun>().WithMany().HasForeignKey(x => x.CollectionRunId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<CollectionSourceAttempt>().WithMany().HasForeignKey(x => x.SourceAttemptId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<IngestionIdempotencyRecord>(entity =>
        {
            entity.ToTable("IngestionIdempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Dataset).HasMaxLength(150).IsRequired();
            entity.Property(x => x.IdempotencyKey).HasMaxLength(300).IsRequired();
            entity.Property(x => x.PayloadHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(40).IsRequired();
            entity.HasIndex(x => new { x.Provider, x.Dataset, x.IdempotencyKey }).IsUnique();
        });

        builder.Entity<IngestionError>(entity =>
        {
            entity.ToTable("IngestionErrors");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Category).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            entity.HasIndex(x => new { x.CollectionRunId, x.OccurredAtUtc });
            entity.HasIndex(x => new { x.IsRetryable, x.NextAttemptAtUtc });
            entity.HasOne<CollectionRun>().WithMany().HasForeignKey(x => x.CollectionRunId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<CollectionSourceAttempt>().WithMany().HasForeignKey(x => x.SourceAttemptId).OnDelete(DeleteBehavior.NoAction);
        });

        ConfigureCloudflare<CloudflareAccountSnapshot>("CloudflareAccountSnapshots");
        ConfigureCloudflare<CloudflareAttentionItem>("CloudflareAttentionItems");
        ConfigureCloudflare<CloudflareCacheInterval>("CloudflareCacheIntervals");
        ConfigureCloudflare<CloudflareCacheDaily>("CloudflareCacheDaily");
        ConfigureCloudflare<CloudflareConfigurationChange>("CloudflareConfigurationChanges");
        ConfigureCloudflare<CloudflareDailyReport>("CloudflareDailyReports");
        ConfigureCloudflare<CloudflareDnsSnapshot>("CloudflareDnsSnapshots");
        ConfigureCloudflare<CloudflareDnssecSnapshot>("CloudflareDnssecSnapshots");
        ConfigureCloudflare<CloudflareHttpError>("CloudflareHttpErrors");
        ConfigureCloudflare<CloudflareHttpErrorDaily>("CloudflareHttpErrorsDaily");
        ConfigureCloudflare<CloudflareOriginInterval>("CloudflareOriginIntervals");
        ConfigureCloudflare<CloudflareOriginHourly>("CloudflareOriginHourly");
        ConfigureCloudflare<CloudflarePerformanceConfigSnapshot>("CloudflarePerformanceConfigSnapshots");
        ConfigureCloudflare<CloudflareRuleSnapshot>("CloudflareRuleSnapshots");
        ConfigureCloudflare<CloudflareSecurityConfigSnapshot>("CloudflareSecurityConfigSnapshots");
        ConfigureCloudflare<CloudflareSecurityDaily>("CloudflareSecurityDaily");
        ConfigureCloudflare<CloudflareSslTlsSnapshot>("CloudflareSslTlsSnapshots");
        ConfigureCloudflare<CloudflareTrafficInterval>("CloudflareTrafficIntervals");
        ConfigureCloudflare<CloudflareTrafficHourly>("CloudflareTrafficHourly");
        ConfigureCloudflare<CloudflareZoneSnapshot>("CloudflareZoneSnapshots");

        builder.Entity<CloudflareDailyReport>(entity =>
        {
            entity.Property(x => x.CollectionStatus).HasMaxLength(40);
            entity.Property(x => x.DataQualityStatus).HasMaxLength(40);
            entity.Property(x => x.ErrorRatePercentage).HasPrecision(18, 6);
            entity.HasIndex(x => new { x.ZoneId, x.ReportDateKey }).IsUnique();
        });

        void ConfigureCloudflare<TEntity>(string tableName) where TEntity : class
        {
            builder.Entity<TEntity>(entity =>
            {
                entity.ToTable(tableName);
                entity.HasKey("Id");
                entity.Property<string>("RecordKey").HasMaxLength(300).IsRequired();
                entity.Property<string>("ZoneId").HasMaxLength(100);
                entity.Property<string>("ZoneName").HasMaxLength(255);
                entity.Property<string>("ReportDateKey").HasMaxLength(40);
                entity.Property<string>("PayloadJson").IsRequired(false);
                entity.Property<DateTime>("CollectedAtUtc");
                entity.HasIndex("RecordKey").IsUnique();
                entity.HasIndex("ZoneId", "ReportDateKey");
            });
        }

        ConfigureGoDaddy<GoDaddyApiHealth>("GoDaddyApiHealth", "HealthKey");
        ConfigureGoDaddy<GoDaddyChange>("GoDaddyChanges", nameof(GoDaddyChange.ChangeKey));
        ConfigureGoDaddy<GoDaddyDomainSnapshot>("GoDaddyDomainSnapshots", nameof(GoDaddyDomainSnapshot.DomainKey));
        ConfigureGoDaddy<GoDaddyIncident>("GoDaddyIncidents", nameof(GoDaddyIncident.IncidentKey));
        ConfigureGoDaddy<GoDaddyMaintenance>("GoDaddyMaintenance", "MaintenanceKey");
        ConfigureGoDaddy<GoDaddyRestApiEvidence>("GoDaddyRestApiEvidence", "ApiKey");
        ConfigureGoDaddy<GoDaddyStatusComponent>("GoDaddyStatusComponents", nameof(GoDaddyStatusComponent.ComponentSnapshotKey));
        ConfigureGoDaddy<GoDaddyStatusSnapshot>("GoDaddyStatusSnapshots", nameof(GoDaddyStatusSnapshot.StatusKey));
        ConfigureGoDaddy<GoDaddyTransaction>("GoDaddyTransactions", nameof(GoDaddyTransaction.TransactionId));

        void ConfigureGoDaddy<TEntity>(string tableName, string keyProperty) where TEntity : class
        {
            builder.Entity<TEntity>(entity =>
            {
                entity.ToTable(tableName);
                entity.HasKey("Id");
                entity.Property<string>(keyProperty).HasMaxLength(300).IsRequired();
                entity.Property<string>("PayloadJson").IsRequired(false);
                entity.HasIndex(keyProperty).IsUnique();
            });
        }

        ConfigureCPanel<CPanelApiEvidence>("CPanelApiEvidence", "EvidenceKey");
        ConfigureCPanel<CPanelAutoSsl>("CPanelAutoSsl", "AutoSslKey");
        ConfigureCPanel<CPanelBackup>("CPanelBackups", nameof(CPanelBackup.BackupKey));
        ConfigureCPanel<CPanelCapability>("CPanelCapabilities", nameof(CPanelCapability.CapabilityKey));
        ConfigureCPanel<CPanelDailyStat>("CPanelDailyStats", nameof(CPanelDailyStat.DailyStatKey));
        ConfigureCPanel<CPanelDatabase>("CPanelDatabases", nameof(CPanelDatabase.DatabaseKey));
        ConfigureCPanel<CPanelResourceDailySummary>("CPanelResourceDailySummaries", nameof(CPanelResourceDailySummary.SummaryKey));
        ConfigureCPanel<CPanelResourceSampleLegacy>("CPanelResourceSamplesLegacy", nameof(CPanelResourceSampleLegacy.SampleKey));
        ConfigureCPanel<CPanelResource>("CPanelResources", nameof(CPanelResource.ResourceKey));
        ConfigureCPanel<CPanelServerError>("CPanelServerErrors", nameof(CPanelServerError.ErrorKey));
        ConfigureCPanel<CPanelServerInfo>("CPanelServerInfo", "ServerKey");
        ConfigureCPanel<CPanelSslCertificate>("CPanelSslCertificates", nameof(CPanelSslCertificate.CertificateKey));
        ConfigureCPanel<CPanelTransaction>("CPanelTransactions", nameof(CPanelTransaction.TransactionId));

        void ConfigureCPanel<TEntity>(string tableName, string keyProperty) where TEntity : class
        {
            builder.Entity<TEntity>(entity =>
            {
                entity.ToTable(tableName);
                entity.HasKey("Id");
                entity.Property<string>(keyProperty).HasMaxLength(300).IsRequired();
                entity.Property<string>("PayloadJson").IsRequired(false);
                entity.HasIndex(keyProperty).IsUnique();
            });
        }

        ConfigureWpUmbrella<WpUmbrellaApiEvidence>("WpUmbrellaApiEvidence", "EvidenceKey");
        ConfigureWpUmbrella<WpUmbrellaAttentionItem>("WpUmbrellaAttentionItems", "AttentionKey");
        ConfigureWpUmbrella<WpUmbrellaBackup>("WpUmbrellaBackups", "BackupKey");
        ConfigureWpUmbrella<WpUmbrellaChange>("WpUmbrellaChanges", nameof(WpUmbrellaChange.ChangeKey));
        ConfigureWpUmbrella<WpUmbrellaDailySummary>("WpUmbrellaDailySummaries", nameof(WpUmbrellaDailySummary.SummaryKey));
        ConfigureWpUmbrella<WpUmbrellaPhpInfo>("WpUmbrellaPhpInfo", "PhpInfoKey");
        ConfigureWpUmbrella<WpUmbrellaPhpIssue>("WpUmbrellaPhpIssues", "IssueKey");
        ConfigureWpUmbrella<WpUmbrellaPlugin>("WpUmbrellaPlugins", nameof(WpUmbrellaPlugin.PluginKey));
        ConfigureWpUmbrella<WpUmbrellaProcess>("WpUmbrellaProcesses", nameof(WpUmbrellaProcess.ProcessKey));
        ConfigureWpUmbrella<WpUmbrellaProjectSnapshot>("WpUmbrellaProjectSnapshots", "ProjectKey");
        ConfigureWpUmbrella<WpUmbrellaTheme>("WpUmbrellaThemes", nameof(WpUmbrellaTheme.ThemeKey));
        ConfigureWpUmbrella<WpUmbrellaUptime>("WpUmbrellaUptime", "UptimeKey");
        ConfigureWpUmbrella<WpUmbrellaUptimeIncident>("WpUmbrellaUptimeIncidents", "IncidentKey");
        ConfigureWpUmbrella<WpUmbrellaVulnerability>("WpUmbrellaVulnerabilities", "VulnerabilityKey");

        void ConfigureWpUmbrella<TEntity>(string tableName, string keyProperty) where TEntity : class
        {
            builder.Entity<TEntity>(entity =>
            {
                entity.ToTable(tableName);
                entity.HasKey("Id");
                entity.Property<string>(keyProperty).HasMaxLength(300).IsRequired();
                entity.Property<string>("PayloadJson").IsRequired(false);
                entity.HasIndex(keyProperty).IsUnique();
            });
        }
    }
}
