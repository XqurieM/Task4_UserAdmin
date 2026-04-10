namespace Task4.UserAdmin.Application.Features.CQRS.Results.AuthResults;

public sealed class VerifyUserEmailResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
