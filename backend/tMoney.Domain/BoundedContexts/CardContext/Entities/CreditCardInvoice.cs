using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CardContext.Entities;

public class CreditCardInvoice
{
    public IdValueObject Id { get; private set; }
    public IdValueObject CreditCardId { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }
    public DateTime CloseDay { get; private set; }
    public DateTime DueDay { get; private set; }
    public decimal LimitTotal { get; private set; }
    public decimal AmountPaid { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private CreditCardInvoice() { }

    public CreditCardInvoice(IdValueObject creditCardId, int month, int year, DateTime closeDay, DateTime dueDay, decimal limitTotal)
    {
        Id = IdValueObject.New();
        CreditCardId = creditCardId;
        Month = month;
        Year = year;
        CloseDay = closeDay;
        DueDay = dueDay;
        LimitTotal = limitTotal;
        AmountPaid = 0;
        Status = InvoiceStatus.Open;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;

        ValidateDomain();
    }

    private void ValidateDomain()
    {
        if (Month < 1 || Month > 12)
            throw new ArgumentException("O mês da fatura deve estar entre 1 e 12.");

        if (Year < 2024 || Year > DateTime.UtcNow.Year + 50)
            throw new ArgumentException("O ano da fatura é inválido.");

        var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var lastDayOfMonth = firstDay.AddMonths(1).AddDays(-1).Day;

        if (CloseDay.Day < 1 || CloseDay.Day > lastDayOfMonth)
            throw new ArgumentException($"O dia de fechamento deve estar entre 1 e {lastDayOfMonth}.");

        if (DueDay.Day < 1 || DueDay.Day > lastDayOfMonth)
            throw new ArgumentException($"O dia de vencimento deve estar entre 1 e {lastDayOfMonth}.");

        if (CloseDay >= DueDay)
            throw new ArgumentException("O dia de fechamento deve ser anterior ao dia de vencimento.");

        if (LimitTotal < 0)
            throw new ArgumentException("O valor do limite não pode ser negativo.");

        if (AmountPaid < 0)
            throw new ArgumentException("O valor pago não pode ser negativo.");

        if (!Enum.IsDefined(typeof(InvoiceStatus), Status))
            throw new ArgumentException("O status da fatura é inválido.");
    }

    public void UpdateLimit(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("O valor do limite não pode ser negativo.");

        if (amount < AmountPaid)
            throw new ArgumentException("O valor do limite não pode ser menor que o valor já pago.");

        LimitTotal = amount;
        UpdatedAt = DateTime.UtcNow;

        ValidateDomain();
    }
}
