using Microsoft.AspNetCore.Identity;
using TravellerAI.Domain.Entities;

namespace TravellerAI.Infrastructure.Db.Mssql.Identity;

/// <summary>
/// Identity user (credentials, roles, lockout). The profile (UserEntity) has the same Id.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public virtual UserEntity? Profile { get; set; }
}
