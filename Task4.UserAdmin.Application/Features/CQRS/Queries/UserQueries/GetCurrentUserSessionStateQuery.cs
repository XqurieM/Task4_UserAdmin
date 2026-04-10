namespace Task4.UserAdmin.Application.Features.CQRS.Queries.UserQueries;

public sealed class GetCurrentUserSessionStateQuery
{
    public int UserId { get; set; }
}
