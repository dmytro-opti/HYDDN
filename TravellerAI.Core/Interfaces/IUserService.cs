using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IUserService
{
    public Task<UserModel> GetUserAsync(Guid userId);
    public Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword);
    public Task UpdateNameAsync(Guid userId, string firstName, string lastName);
    public Task UpdateEmailAsync(Guid userId, string email);
    public Task RemoveUserAsync(Guid userId);
}