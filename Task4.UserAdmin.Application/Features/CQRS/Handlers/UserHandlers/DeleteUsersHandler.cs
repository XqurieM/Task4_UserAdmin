using System.Data;
using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;
using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.UserHandlers;

public sealed class DeleteUsersHandler : Features.CQRS.ICQRS.IDeleteUsers
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public DeleteUsersHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<BulkOperationResult> DeleteUsers(DeleteUsersCommand deleteUsersCommand, int currentUserId)
    {
        if (deleteUsersCommand.UserIds.Count == 0)
        {
            return new BulkOperationResult { Success = false, Message = "You must select at least one user." };
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        var result = await connection.QueryFirstAsync<BulkOperationResult>(
            StoredProcedureNames.UsersDeleteBulk,
            new
            {
                Ids = CreateIdsTable(deleteUsersCommand.UserIds).AsTableValuedParameter("dbo.IntIdList"),
                CurrentUserId = currentUserId
            },
            commandType: CommandType.StoredProcedure);

        result.Success = true;
        result.Message = $"{result.AffectedCount} User deleted.";
        return result;
    }

    private static DataTable CreateIdsTable(IEnumerable<int> ids)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        foreach (var id in ids.Distinct())
        {
            table.Rows.Add(id);
        }
        return table;
    }
}
