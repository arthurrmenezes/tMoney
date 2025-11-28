using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.Application.Services.CategoryContext.Inputs;

public sealed class UpdateCategoryDetailsByTitleServiceInput
{
    public string? NewTitle { get; }
    public CategoryType? NewType { get; }

    private UpdateCategoryDetailsByTitleServiceInput(string? newTitle, CategoryType? newType)
    {
        NewTitle = newTitle;
        NewType = newType;
    }

    public static UpdateCategoryDetailsByTitleServiceInput Factory(string? newTitle, CategoryType? newType)
        => new(newTitle, newType);
}
