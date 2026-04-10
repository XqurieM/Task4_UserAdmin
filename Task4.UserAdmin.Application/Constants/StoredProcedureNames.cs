namespace Task4.UserAdmin.Application.Constants;

public static class StoredProcedureNames
{
    public const string UsersRegister = "dbo.usp_Users_Register";
    public const string UsersGetByNormalizedEmail = "dbo.usp_Users_GetByNormalizedEmail";
    public const string UsersUpdateLoginInfo = "dbo.usp_Users_UpdateLoginInfo";
    public const string UsersVerifyEmail = "dbo.usp_Users_VerifyEmail";
    public const string UsersGetManagementList = "dbo.usp_Users_GetManagementList";
    public const string UsersGetSessionState = "dbo.usp_Users_GetSessionState";
    public const string UsersBlockBulk = "dbo.usp_Users_BlockBulk";
    public const string UsersUnblockBulk = "dbo.usp_Users_UnblockBulk";
    public const string UsersDeleteBulk = "dbo.usp_Users_DeleteBulk";
    public const string UsersDeleteUnverifiedBulk = "dbo.usp_Users_DeleteUnverifiedBulk";
}
