using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.Application.Services.CategoryContext.Inputs;

public sealed class GetAllCategoriesByAccountIdServiceInput
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public CategoryType? CategoryType { get; }
    public string? TextSearch { get; }

    private GetAllCategoriesByAccountIdServiceInput(int pageNumber, int pageSize, CategoryType? categoryType, string? textSearch)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        CategoryType = categoryType;
        TextSearch = textSearch;
    }

    public static GetAllCategoriesByAccountIdServiceInput Factory(int pageNumber, int pageSize, CategoryType? categoryType, string? textSearch)
        => new(pageNumber, pageSize, categoryType, textSearch);
}
