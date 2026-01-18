namespace tMoney.Application.Services.CardContext.Outputs;

public sealed class GetAllCardsByAccountIdServiceOutput
{
    public int TotalCards { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public GetAllCardsByAccountIdServiceOutputCards[] Cards { get; }

    private GetAllCardsByAccountIdServiceOutput(int totalCards, int pageNumber, int pageSize, int totalPages, GetAllCardsByAccountIdServiceOutputCards[] cards)
    {
        TotalCards = totalCards;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        Cards = cards;
    }

    public static GetAllCardsByAccountIdServiceOutput Factory(int totalCards, int pageNumber, int pageSize, int totalPages,
        GetAllCardsByAccountIdServiceOutputCards[] cards)
        => new(totalCards, pageNumber, pageSize, totalPages, cards);
}

public sealed class GetAllCardsByAccountIdServiceOutputCards
{
    public string Id { get; }
    public string AccountId { get; }
    public string Name { get; }
    public string Type { get; }
    public GetAllCardsByAccountIdServiceOutputCreditCard? CreditCard { get; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private GetAllCardsByAccountIdServiceOutputCards(string id, string accountId, string name, string type, 
        GetAllCardsByAccountIdServiceOutputCreditCard? creditCard, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        Name = name;
        Type = type;
        CreditCard = creditCard;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllCardsByAccountIdServiceOutputCards Factory(string id, string accountId, string name, string type,
        GetAllCardsByAccountIdServiceOutputCreditCard? creditCard, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, name, type, creditCard, updatedAt, createdAt);
}

public sealed class GetAllCardsByAccountIdServiceOutputCreditCard
{
    public decimal Limit { get; }
    public int CloseDay { get; }
    public int DueDay { get; }

    private GetAllCardsByAccountIdServiceOutputCreditCard(decimal limit, int closeDay, int dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }

    public static GetAllCardsByAccountIdServiceOutputCreditCard Factory(decimal limit, int closeDay, int dueDay)
        => new(limit, closeDay, dueDay);
}
