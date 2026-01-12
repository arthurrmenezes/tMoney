using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;

namespace tMoney.Application.Services.InstallmentContext.Inputs;

public sealed class UpdateInstallmentDetailsByIdServiceInput
{
    public int? TotalInstallments { get; }
    public decimal? TotalAmount { get; }
    public DateTime? FirstPaymentDate { get; }
    public PaymentStatus? Status { get; }

    private UpdateInstallmentDetailsByIdServiceInput(int? totalInstallments, decimal? totalAmount, DateTime? firstPaymentDate, PaymentStatus? status)
    {
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
    }

    public static UpdateInstallmentDetailsByIdServiceInput Factory(int? totalInstallments, decimal? totalAmount, DateTime? firstPaymentDate, 
        PaymentStatus? status)
        => new(totalInstallments, totalAmount, firstPaymentDate, status);
}
