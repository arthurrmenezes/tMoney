using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CardContext.Entities;

public class CreditCard : Card
{
    public decimal Limit { get; private set; }
    public int CloseDay { get; private set; }
    public int DueDay { get; private set; }

    public CreditCard(IdValueObject accountId, string name, decimal limit, int closeDay, int dueDay) : base(accountId, name)
    {
        Limit = limit;
        CloseDay = closeDay;
        DueDay = dueDay;

        ValidateDomain();
    }
    
    private void ValidateDomain()
    {
        if (Limit <= 0)
            throw new ArgumentException("O valor do limite deve ser maior que 0.");

        if (CloseDay < 1 || CloseDay > 31)
            throw new ArgumentException($"O dia de fechamento deve estar entre 1 e 31.");

        if (DueDay < 1 || DueDay > 31)
            throw new ArgumentException($"O dia de vencimento deve estar entre 1 e 31.");
    }

    public void UpdateCreditCardDetails(decimal? limit, int? closeDay, int? dueDay)
    {
        if (limit is not null)
            Limit = limit.Value;

        if (closeDay is not null)
            CloseDay = closeDay.Value;

        if (dueDay is not null)
            DueDay = dueDay.Value;

        UpdatedAt = DateTime.UtcNow;

        ValidateDomain();
    }
}
