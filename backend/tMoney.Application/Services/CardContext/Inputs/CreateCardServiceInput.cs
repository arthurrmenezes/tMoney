namespace tMoney.Application.Services.CardContext.Inputs;

public sealed class CreateCardServiceInput
{
    public string Name { get; }
    public CreateCardServiceInputCreditCard? CreditCard { get; }

    private CreateCardServiceInput(string name, CreateCardServiceInputCreditCard? creditCard)
    {
        Name = name;
        CreditCard = creditCard;
    }

    public static CreateCardServiceInput Factory(string name, CreateCardServiceInputCreditCard? creditCard)
        => new(name, creditCard);
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
