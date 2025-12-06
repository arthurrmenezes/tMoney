using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.TransactionContext.Inputs;

public sealed class CreateTransactionServiceInput
{
    public IdValueObject AccountId { get; private set; }
    public IdValueObject CategoryId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? Destination { get; private set; }

    private CreateTransactionServiceInput(IdValueObject accountId, IdValueObject categoryId, string title, string? description, decimal amount, 
        DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination)
    {
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
    }

    public static CreateTransactionServiceInput Factory(IdValueObject accountId, IdValueObject categoryId, string title, string? description, decimal amount, 
        DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination)
        => new(accountId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination);
}
