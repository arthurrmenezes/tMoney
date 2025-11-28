using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;

namespace tMoney.Application.Services.CategoryContext.Inputs;

public sealed class CreateCategoryServiceInput
{
    public string Title { get; }
    public CategoryType Type { get; }

    private CreateCategoryServiceInput(string title, CategoryType type)
    {
        Title = title; 
        Type = type; 
    }

    public static CreateCategoryServiceInput Factory(string title, CategoryType type)
        => new(title, type);
}
