using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.WebApi.Controllers.CategoryContext.Payloads;

public sealed class UpdateCategoryDetailsByTitlePayload
{
    public string? NewTitle { get; init; }
    public CategoryType? NewType { get; init; }

    public UpdateCategoryDetailsByTitlePayload(string? newTitle, CategoryType? newType)
    {
        NewTitle = newTitle;
        NewType = newType;
    }
}
