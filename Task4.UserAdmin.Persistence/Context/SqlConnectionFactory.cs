using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task4.UserAdmin.Application.Interfaces;

namespace Task4.UserAdmin.Persistence.Context;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not found.");
    }

    public System.Data.IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
