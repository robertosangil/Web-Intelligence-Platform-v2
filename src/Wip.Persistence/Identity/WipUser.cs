using Microsoft.AspNetCore.Identity;

namespace Wip.Persistence.Identity;

public sealed class WipUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
