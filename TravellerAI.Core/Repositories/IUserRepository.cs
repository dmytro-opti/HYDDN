using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface IUserRepository
{
    public Task<UserEntity> GetUserAsync(Guid userId);
    public Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword);
    public Task UpdateNameAsync(Guid userId, string firstName, string lastName);
    public Task UpdateEmailAsync(Guid userId, string email);
    public Task<Guid> RemoveUserAsync(Guid userId);
}