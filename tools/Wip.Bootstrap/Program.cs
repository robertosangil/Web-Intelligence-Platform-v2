using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wip.Persistence;
using Wip.Persistence.Identity;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: false);
builder.Services.AddPersistence(builder.Configuration);
using var host = builder.Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var db = services.GetRequiredService<WipDbContext>();
var roles = services.GetRequiredService<RoleManager<WipRole>>();
var users = services.GetRequiredService<UserManager<WipUser>>();
const string roleName = "Super Admin";
const string email = "roberto.sangil@offshoremvp.com";
var password = builder.Configuration["Bootstrap:SuperAdminPassword"] ?? throw new InvalidOperationException("Bootstrap password is missing from User Secrets.");
var role = await roles.FindByNameAsync(roleName);
if (role is null)
{
    role = new WipRole { Id = Guid.NewGuid(), Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
    var result = await roles.CreateAsync(role);
    if (!result.Succeeded) throw new InvalidOperationException(string.Join(';', result.Errors.Select(e => e.Code)));
}
var user = await users.FindByEmailAsync(email);
if (user is null)
{
    user = new WipUser { Id = Guid.NewGuid(), UserName = email, Email = email, EmailConfirmed = true, FirstName = "Roberto", LastName = "Sangil" };
    var result = await users.CreateAsync(user, password);
    if (!result.Succeeded) throw new InvalidOperationException(string.Join(';', result.Errors.Select(e => e.Code)));
}
else
{
    user.FirstName = "Roberto"; user.LastName = "Sangil"; user.EmailConfirmed = true; await users.UpdateAsync(user);
}
if (!await users.IsInRoleAsync(user, roleName)) await users.AddToRoleAsync(user, roleName);
var modules = await db.Modules.OrderBy(m => m.GroupKey).ThenBy(m => m.SortOrder).ToListAsync();
var existing = await db.RoleModules.Where(x => x.RoleId == role.Id).Select(x => x.ModuleId).ToHashSetAsync();
foreach (var module in modules.Where(m => !existing.Contains(m.Id))) db.RoleModules.Add(new RoleModule { RoleId = role.Id, ModuleId = module.Id });
await db.SaveChangesAsync();
Console.WriteLine($"Super Admin bootstrap completed. Modules assigned: {modules.Count}.");
