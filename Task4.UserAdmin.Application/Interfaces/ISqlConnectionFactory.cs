using System.Data;

namespace Task4.UserAdmin.Application.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
