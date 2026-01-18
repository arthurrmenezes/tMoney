namespace tMoney.WebApi.Controllers.CardContext.Payloads;

public class CreateCardPayload
{
    public string Name { get; init; }
    public CreateCardPayloadCreditCard? CreditCard { get; init; }

    public CreateCardPayload(string name, CreateCardPayloadCreditCard? creditCard)
    {
        Name = name;
        CreditCard = creditCard;
    }
}

public sealed class CreateCardPayloadCreditCard
{
    public decimal Limit { get; init; }
    public int CloseDay { get; init; }
    public int DueDay { get; init; }

    public CreateCardPayloadCreditCard(decimal limit, int closeDay, int dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }
}
