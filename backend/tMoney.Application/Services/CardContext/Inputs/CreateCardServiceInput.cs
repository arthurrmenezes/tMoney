using tMoney.Domain.BoundedContexts.CardContext.ENUMs;

namespace tMoney.Application.Services.CardContext.Inputs;

public sealed class CreateCardServiceInput
{
    public string Name { get; }
    public CardType CardType { get; }
    public CreateCardServiceInputCreditCard? CreditCard { get; }

    private CreateCardServiceInput(string name, CardType cardType, CreateCardServiceInputCreditCard? creditCard)
    {
        Name = name;
        CardType = cardType;
        CreditCard = creditCard;
    }

    public static CreateCardServiceInput Factory(string name, CardType cardType, CreateCardServiceInputCreditCard? creditCard)
        => new(name, cardType, creditCard);
}

public sealed class CreateCardServiceInputCreditCard
{
    public decimal Limit { get; }
    public int CloseDay { get; }
    public int DueDay { get; }

    private CreateCardServiceInputCreditCard(decimal limit, int closeDay, int dueDay)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;
    }

    public static CreateCardServiceInputCreditCard Factory(decimal limit, int closeDay, int dueDay)
        => new(limit, closeDay, dueDay);
}
