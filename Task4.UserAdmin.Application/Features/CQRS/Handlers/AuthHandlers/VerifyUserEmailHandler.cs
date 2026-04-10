using Dapper;
using Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;
using Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;
using Task4.UserAdmin.Application.Interfaces;
using Task4.UserAdmin.Application.Constants;

namespace Task4.UserAdmin.Application.Features.CQRS.Handlers.AuthHandlers;

public sealed class VerifyUserEmailHandler : Features.CQRS.ICQRS.IVerifyUserEmail
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public VerifyUserEmailHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<VerifyUserEmailResult> VerifyUserEmail(VerifyUserEmailCommand verifyUserEmailCommand)
    {
        if (string.IsNullOrWhiteSpace(verifyUserEmailCommand.Token))
        {
            return new VerifyUserEmailResult { Success = false, Message = "Verification key not found." };
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<VerifyResultDto>(
            StoredProcedureNames.UsersVerifyEmail,
            new { Token = verifyUserEmailCommand.Token.Trim() },
            commandType: System.Data.CommandType.StoredProcedure);

        if (result is null)
        {
            return new VerifyUserEmailResult { Success = false, Message = "Invalid or expired verification link." };
        }

        return new VerifyUserEmailResult
        {
            Success = result.Success,
            Message = result.Message
        };
    }

    private sealed class VerifyResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
