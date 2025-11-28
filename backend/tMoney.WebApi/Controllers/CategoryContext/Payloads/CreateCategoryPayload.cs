using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.WebApi.Controllers.CategoryContext.Payloads;

public sealed class CreateCategoryPayload
{
    public string Title { get; init; }
    public CategoryType Type { get; init; }

    public CreateCategoryPayload(string title, CategoryType type)
    {
        Title = title;
        Type = type;
    }
}
