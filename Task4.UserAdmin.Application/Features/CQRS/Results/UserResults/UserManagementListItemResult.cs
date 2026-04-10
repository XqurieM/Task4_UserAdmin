namespace Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;

public sealed class UserManagementListItemResult
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }
    public DateTime? LastActivityAtUtc { get; set; }
}
