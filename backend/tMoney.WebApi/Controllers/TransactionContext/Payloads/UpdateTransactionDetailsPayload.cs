using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;

namespace tMoney.WebApi.Controllers.TransactionContext.Payloads;

public sealed class UpdateTransactionDetailsPayload
{
    public Guid? CategoryId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public decimal? Amount { get; init; }
    public DateTime? Date { get; init; }
    public PaymentStatus? Status { get; init; }
    public string? Destination { get; init; }

    public UpdateTransactionDetailsPayload(Guid? categoryId, string? title, string? description, decimal? amount, DateTime? date, PaymentStatus? status, 
        string? destination)
    {
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        Status = status;
        Destination = destination;
    }
}
