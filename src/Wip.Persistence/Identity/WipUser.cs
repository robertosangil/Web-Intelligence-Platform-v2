using Microsoft.AspNetCore.Identity;

namespace Wip.Persistence.Identity;

public sealed class WipUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? DeactivatedAtUtc { get; set; }
}
