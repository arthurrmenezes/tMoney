using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.InstallmentContext.Entities;

public class InstallmentItem
{
    public IdValueObject Id { get; private set; }
    public IdValueObject InstallmentId { get; private set; }
    public IdValueObject? InvoiceId { get; private set; }
    public int Number { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DueDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected InstallmentItem() { }

    public InstallmentItem(IdValueObject installmentId, IdValueObject? invoiceId, int number, decimal amount, DateTime dueDate, 
        PaymentStatus status = PaymentStatus.Pending)
    {
        Id = IdValueObject.New();
        InvoiceId = invoiceId;
        InstallmentId = installmentId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = null;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }

    public void PayInstallment()
    {
        Status = PaymentStatus.Paid;

        PaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
