using Microsoft.EntityFrameworkCore;

namespace Wip.Persistence;

public sealed class WipDbContext(DbContextOptions<WipDbContext> options) : DbContext(options);
