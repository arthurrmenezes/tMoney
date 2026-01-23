using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.InstallmentContext.Inputs;

public sealed class CreateInstallmentServiceInput
{
    public IdValueObject AccountId { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public PaymentStatus Status { get; }

    public CreateInstallmentServiceInput(IdValueObject accountId, int totalInstallments, decimal totalAmount, 
        DateTime firstPaymentDate, PaymentStatus status)
    {
        AccountId = accountId;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
    }

    public static CreateInstallmentServiceInput Factory(IdValueObject accountId, int totalInstallments, decimal totalAmount, 
        DateTime firstPaymentDate, PaymentStatus status)
        => new(accountId, totalInstallments, totalAmount, firstPaymentDate, status);
}
