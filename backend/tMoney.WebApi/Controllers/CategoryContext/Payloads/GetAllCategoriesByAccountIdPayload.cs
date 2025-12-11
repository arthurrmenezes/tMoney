using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.WebApi.Controllers.CategoryContext.Payloads;

public sealed class GetAllCategoriesByAccountIdPayload
{
    public CategoryType? CategoryType { get; init; }
    public string? TextSearch { get; init; }

    public GetAllCategoriesByAccountIdPayload() { }

    public GetAllCategoriesByAccountIdPayload(CategoryType? categoryType, string? textSearch)
    {
        CategoryType = categoryType;
        TextSearch = textSearch;
    }
}
