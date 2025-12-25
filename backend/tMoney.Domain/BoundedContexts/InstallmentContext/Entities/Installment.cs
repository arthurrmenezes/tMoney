using tMoney.Domain.BoundedContexts.InstallmentContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.InstallmentContext.Entities;

public class Installment
{
    public IdValueObject Id { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public int TotalInstallments { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal InterestRate { get; private set; }
    public DateTime FirstPaymentDate { get; private set; }
    public InstallmentStatus Status { get; private set; }
    public List<InstallmentItem> Installments { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Installment() { }

    public Installment(IdValueObject accountId, int totalInstallments, decimal totalAmount, decimal interestRate, DateTime firstPaymentDate, 
        InstallmentStatus status, DateTime? updatedAt, DateTime createdAt)
    {
        Id = IdValueObject.New();
        AccountId = accountId;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        InterestRate = interestRate;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        Installments = new List<InstallmentItem>(); ;
        UpdatedAt = updatedAt;
        CreatedAt = DateTime.UtcNow;
    }
}
