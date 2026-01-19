using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;

namespace tMoney.WebApi.Controllers.TransactionContext.Payloads;

public sealed class CreateTransactionPayload
{
    public Guid CardId { get; init; }
    public Guid CategoryId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public TransactionType TransactionType { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public PaymentStatus Status { get; init; }
    public string? Destination { get; init; }
    public CreateTransactionPayloadInstallment? HasInstallment { get; init; }

    public CreateTransactionPayload(Guid cardId, Guid categoryId, string title, string? description, decimal amount, DateTime date, 
        TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination, 
        CreateTransactionPayloadInstallment? hasInstallment)
    {
        CardId = cardId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        HasInstallment = hasInstallment;
    }
}

public sealed class CreateTransactionPayloadInstallment
{
    public int TotalInstallments { get; init; }

    public CreateTransactionPayloadInstallment(int totalInstallments)
    {
        TotalInstallments = totalInstallments;
    }
}
