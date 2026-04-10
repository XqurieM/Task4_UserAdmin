namespace Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;

public sealed class AuthenticateUserResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public bool IsEmailVerified { get; set; }
}
