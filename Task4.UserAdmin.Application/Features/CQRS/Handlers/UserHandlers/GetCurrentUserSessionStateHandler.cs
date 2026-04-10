using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;
using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.UserHandlers;

public sealed class GetCurrentUserSessionStateHandler : Features.CQRS.ICQRS.IGetCurrentUserSessionState
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCurrentUserSessionStateHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<CurrentUserSessionStateResult?> GetCurrentUserSessionState(GetCurrentUserSessionStateQuery getCurrentUserSessionStateQuery)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<CurrentUserSessionStateResult>(
            StoredProcedureNames.UsersGetSessionState,
            new { UserId = getCurrentUserSessionStateQuery.UserId },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
