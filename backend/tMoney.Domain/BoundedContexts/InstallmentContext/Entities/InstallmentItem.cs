using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.InstallmentContext.Entities;

public class InstallmentItem
{
    public IdValueObject Id { get; private set; }
    public IdValueObject InstallmentId { get; private set; }
    public int Number { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DueDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected InstallmentItem() { }

    public InstallmentItem(IdValueObject installmentId, int number, decimal amount, DateTime dueDate)
    {
        Id = IdValueObject.New();
        InstallmentId = installmentId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = PaymentStatus.Pending;
        PaidAt = null;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }

    public void PayInstallment()
    {
        Status = PaymentStatus.Paid;
    }
}
