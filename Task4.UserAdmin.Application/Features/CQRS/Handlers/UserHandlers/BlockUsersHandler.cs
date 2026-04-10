using System.Data;
using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;
using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.UserHandlers;

public sealed class BlockUsersHandler : Features.CQRS.ICQRS.IBlockUsers
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public BlockUsersHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<BulkOperationResult> BlockUsers(BlockUsersCommand blockUsersCommand, int currentUserId)
    {
        if (blockUsersCommand.UserIds.Count == 0)
        {
            return new BulkOperationResult { Success = false, Message = "You must select at least one user." };
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        var result = await connection.QueryFirstAsync<BulkOperationResult>(
            StoredProcedureNames.UsersBlockBulk,
            new
            {
                Ids = CreateIdsTable(blockUsersCommand.UserIds).AsTableValuedParameter("dbo.IntIdList"),
                CurrentUserId = currentUserId
            },
            commandType: CommandType.StoredProcedure);

        result.Success = true;
        result.Message = $"{result.AffectedCount} User blocked.";
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
