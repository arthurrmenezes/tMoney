namespace tMoney.Application.Services.CardContext.Inputs;

public sealed class UpdateCardByIdServiceInput
{
    public string? Name { get; }
    public UpdateCardByIdServiceInputCreditCard? CreditCard { get; }

    private UpdateCardByIdServiceInput(string? name, UpdateCardByIdServiceInputCreditCard? creditCard)
    {
        Name = name;
        CreditCard = creditCard;
    }

    public static UpdateCardByIdServiceInput Factory(string? name, UpdateCardByIdServiceInputCreditCard? creditCard)
        => new(name, creditCard);
}

public sealed class UpdateCardByIdServiceInputCreditCard
{
    public decimal? Limit { get; }
    public int? CloseDay { get; }
    public int? DueDay { get; }

    private UpdateCardByIdServiceInputCreditCard(decimal? limit, int? closeDay, int? dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }

    public static UpdateCardByIdServiceInputCreditCard Factory(decimal? limit, int? closeDay, int? dueDay)
        => new(limit, closeDay, dueDay);
}
