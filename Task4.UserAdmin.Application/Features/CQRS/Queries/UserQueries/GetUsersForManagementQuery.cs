namespace Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;

public sealed class GetUsersForManagementQuery
{
    public string? SearchTerm { get; set; }
}
