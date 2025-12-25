using tMoney.Domain.BoundedContexts.InstallmentContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.InstallmentContext.Entities;

public class InstallmentItem
{
    public IdValueObject Id { get; private set; }
    public IdValueObject InstallmentId { get; private set; }
    public int Number { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DueDate { get; private set; }
    public InstallmentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected InstallmentItem() { }

    public InstallmentItem(IdValueObject installmentId, int number, decimal amount, DateTime dueDate, InstallmentStatus status, DateTime? paidAt,
        DateTime? updatedAt)
    {
        Id = IdValueObject.New();
        InstallmentId = installmentId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = DateTime.UtcNow;
    }
}
