using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

/// <summary>
/// User profiles. Credentials and email changes are handled by IIdentityService.
/// </summary>
public interface IUserRepository : IRepository<UserEntity>
{
    public Task<UserEntity?> GetUserAsync(Guid userId);
    public Task UpdateNameAsync(Guid userId, string firstName, string lastName);
}
