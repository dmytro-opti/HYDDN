using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Exceptions;
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

    public async Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await GetRequiredUserAsync(userId);

        // TODO: compare password hashes once hashing is introduced in AuthService
        if (user.Password != oldPassword)
        {
            return false;
        }

        user.Password = newPassword;
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        var user = await GetRequiredUserAsync(userId);

        user.FirstName = firstName;
        user.LastName = lastName;
        await Context.SaveChangesAsync();
    }

    public async Task UpdateEmailAsync(Guid userId, string email)
    {
        var user = await GetRequiredUserAsync(userId);

        user.Email = email;
        user.IsEmailConfirmed = false;
        await Context.SaveChangesAsync();
    }

    public async Task<Guid> RemoveUserAsync(Guid userId)
    {
        if (!await DeleteAsync(userId))
        {
            throw new ResourceNotFoundException($"User {userId} not found");
        }

        return userId;
    }

    public Task<bool> IsEmailTakenAsync(string email, Guid? exceptUserId = null)
    {
        return DbSet.AnyAsync(u => u.Email == email && u.Id != exceptUserId);
    }

    private async Task<UserEntity> GetRequiredUserAsync(Guid userId)
    {
        return await GetByIdAsync(userId)
               ?? throw new ResourceNotFoundException($"User {userId} not found");
    }
}
