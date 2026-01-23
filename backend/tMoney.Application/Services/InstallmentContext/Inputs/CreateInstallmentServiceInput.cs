using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.InstallmentContext.Inputs;

public sealed class CreateInstallmentServiceInput
{
    public IdValueObject AccountId { get; }
    public IdValueObject[]? InvoiceIds { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public PaymentStatus Status { get; }

    public CreateInstallmentServiceInput(IdValueObject accountId, IdValueObject[]? invoiceIds, int totalInstallments, decimal totalAmount, 
        DateTime firstPaymentDate, PaymentStatus status)
    {
        AccountId = accountId;
        InvoiceIds = invoiceIds;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
    }

    public static CreateInstallmentServiceInput Factory(IdValueObject accountId, IdValueObject[]? invoiceIds, int totalInstallments, decimal totalAmount, 
        DateTime firstPaymentDate, PaymentStatus status)
        => new(accountId, invoiceIds, totalInstallments, totalAmount, firstPaymentDate, status);
}
