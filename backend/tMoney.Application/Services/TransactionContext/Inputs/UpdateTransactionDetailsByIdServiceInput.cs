using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.TransactionContext.Inputs;

public sealed class UpdateTransactionDetailsByIdServiceInput
{
    public IdValueObject? CategoryId { get; }
    public IdValueObject? InstallmentId { get; }
    public string? Title { get; }
    public string? Description { get; }
    public decimal? Amount { get; }
    public DateTime? Date { get; }
    public TransactionType? TransactionType { get; }
    public PaymentMethod? PaymentMethod { get; }
    public PaymentStatus? Status { get; }
    public string? Destination { get; }

    private UpdateTransactionDetailsByIdServiceInput(IdValueObject? categoryId, IdValueObject? installmentId, string? title, string? description, 
        decimal? amount, DateTime? date, TransactionType? transactionType, PaymentMethod? paymentMethod, PaymentStatus? status, string? destination)
    {
        CategoryId = categoryId;
        InstallmentId = installmentId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
    }

    public static UpdateTransactionDetailsByIdServiceInput Factory(IdValueObject? categoryId, IdValueObject? installmentId, string? title, string? description, 
        decimal? amount, DateTime? date, TransactionType? transactionType, PaymentMethod? paymentMethod, PaymentStatus? status, string? destination)
        => new(categoryId, installmentId, title, description, amount, date, transactionType, paymentMethod, status, destination);
}
