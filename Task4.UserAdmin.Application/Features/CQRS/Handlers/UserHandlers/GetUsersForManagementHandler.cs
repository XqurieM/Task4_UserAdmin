using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;
using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.UserHandlers;

public sealed class GetUsersForManagementHandler : Features.CQRS.ICQRS.IGetUsersForManagement
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUsersForManagementHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<List<UserManagementListItemResult>> GetUsersForManagement(GetUsersForManagementQuery getUsersForManagementQuery)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        var users = await connection.QueryAsync<UserManagementListItemResult>(
            StoredProcedureNames.UsersGetManagementList,
            new { SearchTerm = getUsersForManagementQuery.SearchTerm },
            commandType: System.Data.CommandType.StoredProcedure);

        return users.ToList();
    }
}
