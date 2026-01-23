using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CardContext.Entities;

public class CreditCard : Card
{
    public decimal Limit { get; private set; }
    public int CloseDay { get; private set; }
    public int DueDay { get; private set; }

    public CreditCard(IdValueObject accountId, string name, decimal limit, int closeDay, int dueDay) : base(accountId, name)
    {
        Type = CardType.CreditCard;
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

    public CreditCardInvoice GenerateInvoice(DateTime referenceDate)
    {
        var month = referenceDate.Month;
        var year = referenceDate.Year;

        if (referenceDate.Day >= CloseDay)
        {
            if (month == 12)
            {
                month = 1;
                year++;
            }
            else
            {
                month++;
            }
        }

        var daysInCloseMonth = DateTime.DaysInMonth(year, month);
        var closeDay = Math.Min(CloseDay, daysInCloseMonth);
        var closeDate = new DateTime(year, month, closeDay, 0, 0, 0, DateTimeKind.Utc);

        var dueMonth = month;
        var dueYear = year;

        if (DueDay < CloseDay)
        {
            if (dueMonth == 12)
            {
                dueMonth = 1;
                dueYear++;
            }
            else
            {
                dueMonth++;
            }
        }

        var daysInDueMonth = DateTime.DaysInMonth(dueYear, dueMonth);
        var dueDay = Math.Min(DueDay, daysInDueMonth);
        var dueDate = new DateTime(dueYear, dueMonth, dueDay, 0, 0, 0, DateTimeKind.Utc);

        return new CreditCardInvoice(
            creditCardId: Id,
            month: month,
            year: year,
            closeDay: closeDate,
            dueDay: dueDate,
            limitTotal: Limit);
    }
}
