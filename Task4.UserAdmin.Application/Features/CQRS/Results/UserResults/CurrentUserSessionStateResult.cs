namespace Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;

public sealed class CurrentUserSessionStateResult
{
    public bool Exists { get; set; }
    public bool IsBlocked { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
}
