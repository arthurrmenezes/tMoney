namespace tMoney.Application.Services.CardContext.Outputs;

public sealed class GetCardByIdServiceOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public string Name { get; }
    public string Type { get; }
    public GetCardByIdServiceOutputCreditCard? CreditCard { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetCardByIdServiceOutput(string id, string accountId, string name, string type, GetCardByIdServiceOutputCreditCard? creditCard, 
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

    public static GetCardByIdServiceOutput Factory(string id, string accountId, string name, string type, GetCardByIdServiceOutputCreditCard? creditCard, 
        DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, name, type, creditCard, updatedAt, createdAt);
}

public sealed class GetCardByIdServiceOutputCreditCard
{
    public decimal Limit { get; }
    public int CloseDay { get; }
    public int DueDay { get; }

    private GetCardByIdServiceOutputCreditCard(decimal limit, int closeDay, int dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }

    public static GetCardByIdServiceOutputCreditCard Factory(decimal limit, int closeDay, int dueDay)
        => new(limit, closeDay, dueDay);
}
