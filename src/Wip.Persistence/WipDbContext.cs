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
            entity.HasIndex(module => module.Key).IsUnique();
        });

        builder.Entity<RoleModule>(entity =>
        {
            entity.ToTable("RoleModules");
            entity.HasKey(roleModule => new { roleModule.RoleId, roleModule.ModuleId });
            entity.HasOne<WipRole>().WithMany().HasForeignKey(roleModule => roleModule.RoleId);
            entity.HasOne<Module>().WithMany().HasForeignKey(roleModule => roleModule.ModuleId);
        });
    }
}
