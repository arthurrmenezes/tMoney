namespace tMoney.Application.Services.CategoryContext.Outputs;

public sealed class UpdateCategoryDetailsByIdServiceOutput
{
    public string Id { get; }
    public string Title { get; }
    public string Type { get; }
    public string AccountId { get; }
    public DateTime UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private UpdateCategoryDetailsByIdServiceOutput(string id, string title, string type, string accountId, DateTime updatedAt, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Type = type;
        AccountId = accountId;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static UpdateCategoryDetailsByIdServiceOutput Factory(string id, string title, string type, string accountId, DateTime updatedAt, DateTime createdAt)
        => new(id, title, type, accountId, updatedAt, createdAt);
}
