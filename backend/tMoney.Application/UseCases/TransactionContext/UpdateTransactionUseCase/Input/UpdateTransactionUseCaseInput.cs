using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;

public sealed class UpdateTransactionUseCaseInput
{
    public IdValueObject TransactionId { get; }
    public IdValueObject AccountId { get; }
    public IdValueObject? CategoryId { get; }
    //public IdValueObject[]? InvoiceIds { get; }
    public string? Title { get; }
    public string? Description { get; }
    public decimal? Amount { get; }
    public DateTime? Date { get; }
    public TransactionType? TransactionType { get; }
    public PaymentMethod? PaymentMethod { get; }
    public PaymentStatus? Status { get; }
    public string? Destination { get; }
    public UpdateTransactionUseCaseInputInstallment? Installment { get; }

    private UpdateTransactionUseCaseInput(IdValueObject transactionId, IdValueObject accountId, IdValueObject? categoryId, 
        string? title, string? description, decimal? amount, DateTime? date, TransactionType? transactionType, PaymentMethod? paymentMethod, 
        PaymentStatus? status, string? destination, UpdateTransactionUseCaseInputInstallment? installment)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        Installment = installment;
    }

    public static UpdateTransactionUseCaseInput Factory(IdValueObject transactionId, IdValueObject accountId, IdValueObject? categoryId, 
         string? title, string? description, decimal? amount, DateTime? date, TransactionType? transactionType, 
        PaymentMethod? paymentMethod, PaymentStatus? status, string? destination, UpdateTransactionUseCaseInputInstallment? installment)
        => new(transactionId, accountId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, 
            installment);
}

public sealed class UpdateTransactionUseCaseInputInstallment
{
    public int? TotalInstallments { get; }

    public UpdateTransactionUseCaseInputInstallment(int? totalInstallments)
    {
        TotalInstallments = totalInstallments;
    }
}
