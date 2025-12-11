using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.WebApi.Controllers.TransactionContext.Payloads;

public class GetAllTransactionsPayload
{
    public TransactionType? TransactionType { get; init; }
    public IdValueObject? CategoryId { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal? MinValue { get; init; }
    public decimal? MaxValue { get; init; }
    public string? TextSearch { get; init; }

    public GetAllTransactionsPayload() { }

    public GetAllTransactionsPayload(TransactionType? transactionType, IdValueObject? categoryId, PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, 
        DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, string? textSearch)
    {
        TransactionType = transactionType;
        CategoryId = categoryId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        StartDate = startDate;
        EndDate = endDate;
        MinValue = minValue;
        MaxValue = maxValue;
        TextSearch = textSearch;
    }
}
