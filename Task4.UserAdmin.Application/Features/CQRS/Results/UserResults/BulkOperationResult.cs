namespace Task4.UserAdmin.Application.Features.CQRS.Results.UserResults;

public sealed class BulkOperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int AffectedCount { get; set; }
    public int IgnoredCount { get; set; }
    public bool TouchedCurrentUser { get; set; }
}
