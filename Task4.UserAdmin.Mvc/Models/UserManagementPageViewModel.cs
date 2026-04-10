using Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;

namespace Task4.UserAdmin.Mvc.Models;

public sealed class UserManagementPageViewModel
{
    public string? SearchTerm { get; set; }
    public IReadOnlyCollection<UserManagementListItemResult> Users { get; set; } = Array.Empty<UserManagementListItemResult>();
}
