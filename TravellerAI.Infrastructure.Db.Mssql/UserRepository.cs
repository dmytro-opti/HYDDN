using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;

namespace TravellerAI.Infrastructure.Db.Mssql;

public class UserRepository : IUserRepository
{
    public async Task<UserEntity> GetUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateEmailAsync(Guid userId, string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> RemoveUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}