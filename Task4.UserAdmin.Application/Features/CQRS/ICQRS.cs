using Task4.UserAdmin.Application.Features.CQRS.Commands.AuthCommands;
using Task4.UserAdmin.Application.Features.CQRS.Commands.UserCommands;
using Task4.UserAdmin.Application.Features.CQRS.Queries.AuthQueries;
using Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;
using Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;
using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;

namespace Task4.UserAdmin.Application.Features.CQRS;

public static class ICQRS
{
    #region Auth
    public interface IRegisterUser
    {
        Task<RegisterUserResult> RegisterUser(RegisterUserCommand registerUserCommand);
    }

    public interface IAuthenticateUser
    {
        Task<AuthenticateUserResult> AuthenticateUser(AuthenticateUserQuery authenticateUserQuery);
    }

    public interface IVerifyUserEmail
    {
        Task<VerifyUserEmailResult> VerifyUserEmail(VerifyUserEmailCommand verifyUserEmailCommand);
    }
    #endregion

    #region User Management Queries
    public interface IGetUsersForManagement
    {
        Task<List<UserManagementListItemResult>> GetUsersForManagement(GetUsersForManagementQuery getUsersForManagementQuery);
    }

    public interface IGetCurrentUserSessionState
    {
        Task<CurrentUserSessionStateResult?> GetCurrentUserSessionState(GetCurrentUserSessionStateQuery getCurrentUserSessionStateQuery);
    }
    #endregion

    #region User Management Commands
    public interface IBlockUsers
    {
        Task<BulkOperationResult> BlockUsers(BlockUsersCommand blockUsersCommand, int currentUserId);
    }

    public interface IUnblockUsers
    {
        Task<BulkOperationResult> UnblockUsers(UnblockUsersCommand unblockUsersCommand, int currentUserId);
    }

    public interface IDeleteUsers
    {
        Task<BulkOperationResult> DeleteUsers(DeleteUsersCommand deleteUsersCommand, int currentUserId);
    }

    public interface IDeleteUnverifiedUsers
    {
        Task<BulkOperationResult> DeleteUnverifiedUsers(DeleteUnverifiedUsersCommand deleteUnverifiedUsersCommand, int currentUserId);
    }
    #endregion
}
