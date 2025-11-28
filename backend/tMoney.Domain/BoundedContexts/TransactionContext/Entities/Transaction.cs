using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.TransactionContext.Entities;

public class Transaction
{
    public IdValueObject Id { get; private set; }
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
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Transaction(IdValueObject accountId, IdValueObject categoryId, string title, string? description, decimal amount, DateTime date, 
        TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination)
    {
        Id = IdValueObject.New();
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
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }
}
