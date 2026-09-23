using TravellerAI.Domain.Entities;

namespace TravellerAI.Infrastructure.Db.Mssql.Identity;

/// <summary>
/// Refresh token of a user session. Only the SHA-256 hash of the token is stored.
/// </summary>
public class RefreshTokenEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}
