using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TravellerAI.Infrastructure.Db.Mssql;

public abstract class MssqlRepositoryBase
{
    private readonly string _connectionString;

    protected MssqlRepositoryBase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("The connection string 'DefaultConnection' was not found.");
    }

    protected IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
