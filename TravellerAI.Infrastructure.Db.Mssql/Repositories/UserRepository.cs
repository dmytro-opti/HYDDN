using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;

namespace TravellerAI.Infrastructure.Db.Mssql;

public class UserRepository : MssqlRepositoryBase, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }
    
    public async Task<UserEntity> GetUserAsync(Guid userId)
    {
        const string sql = @"
            SELECT Id, Name, FirstName, LastName, Password, Email, IsEmailConfirmed
            FROM dbo.Users
            WHERE Id = @UserId";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserEntity>(sql, new { UserId = userId });
    }

    public async Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        const string selectSql = @"
            SELECT Password
            FROM dbo.Users
            WHERE Id = @UserId";

        using var connection = CreateConnection();
        var currentPassword = await connection.QuerySingleOrDefaultAsync<string?>(selectSql, new { UserId = userId });
        if (currentPassword is null || currentPassword != oldPassword)
        {
            return false;
        }

        const string updateSql = @"
            UPDATE dbo.Users
            SET Password = @NewPassword
            WHERE Id = @UserId";

        var rows = await connection.ExecuteAsync(updateSql, new { UserId = userId, NewPassword = newPassword });
        return rows > 0;
    }

    public async Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        const string sql = @"
            UPDATE dbo.Users
            SET FirstName = @FirstName,
                LastName = @LastName
            WHERE Id = @UserId";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId, FirstName = firstName, LastName = lastName });
    }

    public async Task UpdateEmailAsync(Guid userId, string email)
    {
        const string sql = @"
            UPDATE dbo.Users
            SET Email = @Email
            WHERE Id = @UserId";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId, Email = email });
    }

    public async Task<Guid> RemoveUserAsync(Guid userId)
    {
        const string sql = @"
            DELETE FROM dbo.Users
            WHERE Id = @UserId";

        using var connection = CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { UserId = userId });
        return rows > 0 ? userId : Guid.Empty;
    }
}