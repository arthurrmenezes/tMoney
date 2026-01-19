namespace tMoney.WebApi.Controllers.CardContext.Payloads;

public sealed class UpdateCardByIdPayload
{
    public string? Name { get; init; }
    public UpdateCardByIdPayloadCreditCard? CreditCard { get; init; }

    public UpdateCardByIdPayload(string? name, UpdateCardByIdPayloadCreditCard? creditCard)
    {
        Name = name;
        CreditCard = creditCard;
    }
}

public sealed class UpdateCardByIdPayloadCreditCard
{
    public decimal? Limit { get; init; }
    public int? CloseDay { get; init; }
    public int? DueDay { get; init; }

    public UpdateCardByIdPayloadCreditCard(decimal? limit, int? closeDay, int? dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }
}
