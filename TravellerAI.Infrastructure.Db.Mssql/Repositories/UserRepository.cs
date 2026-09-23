using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(TravellerDbContext context) : base(context)
    {
    }

    public Task<UserEntity?> GetUserAsync(Guid userId)
    {
        return GetByIdAsync(userId);
    }

    public async Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        var user = await GetByIdAsync(userId) ?? throw new NotFoundException("User", userId);

        user.FirstName = firstName;
        user.LastName = lastName;
        await SaveChangesAsync();
    }
}
