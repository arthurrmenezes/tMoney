namespace tMoney.Application.Services.CardContext.Outputs;

public sealed class CreateCardServiceOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public string Name { get; }
    public string Type { get; }
    public CreateCardServiceOutputCreditCard? CreditCard { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private CreateCardServiceOutput(string id, string accountId, string name, string type, CreateCardServiceOutputCreditCard? creditCard, 
        DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        Name = name;
        Type = type;
        CreditCard = creditCard;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static CreateCardServiceOutput Factory(string id, string accountId, string name, string type, CreateCardServiceOutputCreditCard? creditCard, 
        DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, name, type, creditCard, updatedAt, createdAt);
}

public sealed class CreateCardServiceOutputCreditCard
{
    public decimal Limit { get; }
    public int CloseDay { get; }
    public int DueDay { get; }

    private CreateCardServiceOutputCreditCard(decimal limit, int closeDay, int dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }

    public static CreateCardServiceOutputCreditCard Factory(decimal limit, int closeDay, int dueDay)
        => new(limit, closeDay, dueDay);
}
