namespace tMoney.Application.Services.CategoryContext.Outputs;

public sealed class GetAllCategoriesByAccountIdServiceOutput
{
    public int TotalCategories { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public GetAllCategoriesByAccountIdServiceOutputCategory[] Categories { get; }

    private GetAllCategoriesByAccountIdServiceOutput(int totalCategories, int pageNumber, int pageSize, int totalPages, 
        GetAllCategoriesByAccountIdServiceOutputCategory[] categories)
    {
        TotalCategories = totalCategories;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        Categories = categories;
    }

    public static GetAllCategoriesByAccountIdServiceOutput Factory(int totalCategories, int pageNumber, int pageSize, int totalPages,
        GetAllCategoriesByAccountIdServiceOutputCategory[] categories)
        => new(totalCategories, pageNumber, pageSize, totalPages, categories);
}

public sealed class GetAllCategoriesByAccountIdServiceOutputCategory
{
    public string Id { get; }
    public string Title { get; }
    public string Type { get; }
    public string AccountId { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public GetAllCategoriesByAccountIdServiceOutputCategory(string id, string title, string type, string accountId, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Type = type;
        AccountId = accountId;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }
}
