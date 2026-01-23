using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.WebApi.Controllers.TransactionContext.Payloads;

public class GetAllTransactionsPayload
{
    public Guid? CardId { get; init; }
    public TransactionType? TransactionType { get; init; }
    public Guid? CategoryId { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal? MinValue { get; init; }
    public decimal? MaxValue { get; init; }
    public string? TextSearch { get; init; }
    public bool? HasInstallment { get; init; }

    public GetAllTransactionsPayload() { }

    public GetAllTransactionsPayload(Guid? cardId, TransactionType? transactionType, Guid? categoryId, PaymentMethod? paymentMethod, 
        PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, string? textSearch, bool? hasInstallment)
    {
        CardId = cardId;
        TransactionType = transactionType;
        CategoryId = categoryId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        StartDate = startDate;
        EndDate = endDate;
        MinValue = minValue;
        MaxValue = maxValue;
        TextSearch = textSearch;
        HasInstallment = hasInstallment;
    }
}
