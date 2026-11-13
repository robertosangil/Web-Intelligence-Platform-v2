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
    }
}
