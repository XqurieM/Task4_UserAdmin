namespace Task4.UserAdmin.Mvc.Models;

public sealed class BulkActionRequest
{
    public List<int> SelectedUserIds { get; set; } = new();
    public string? SearchTerm { get; set; }
}
