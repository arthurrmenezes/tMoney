using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.TransactionContext.Inputs;

public sealed class CreateTransactionServiceInput
{
    public IdValueObject AccountId { get; }
    public IdValueObject CardId { get; }
    public IdValueObject CategoryId { get; }
    public IdValueObject? InstallmentId { get; }
    public IdValueObject? InvoiceId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public TransactionType TransactionType { get; }
    public PaymentMethod PaymentMethod { get; }
    public PaymentStatus Status { get; }
    public string? Destination { get; }

    private CreateTransactionServiceInput(IdValueObject accountId, IdValueObject cardId, IdValueObject categoryId, IdValueObject? installmentId, 
        IdValueObject? invoiceId, string title, string? description, decimal amount, DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, 
        PaymentStatus status, string? destination)
    {
        AccountId = accountId;
        CardId = cardId;
        CategoryId = categoryId;
        InstallmentId = installmentId;
        InvoiceId = invoiceId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
    }

    public static CreateTransactionServiceInput Factory(IdValueObject accountId, IdValueObject cardId, IdValueObject categoryId, IdValueObject? installmentId,
        IdValueObject? invoiceId, string title, string? description, decimal amount, DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, 
        PaymentStatus status, string? destination)
        => new(accountId, cardId, categoryId, installmentId, invoiceId, title, description, amount, date, transactionType, paymentMethod, status, destination);
}
