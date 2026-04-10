namespace Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;

public sealed class RegisterUserResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? UserId { get; set; }
}
