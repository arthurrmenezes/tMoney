using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CategoryContext.Entities;

public class Category
{
    public IdValueObject Id { get; private set; }
    public string Title { get; private set; }
    public CategoryType Type { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Category() { }

    public Category(string title, CategoryType type, IdValueObject accountId)
    {
        Id = IdValueObject.New();
        Title = title;
        Type = type;
        AccountId = accountId;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }
}
