using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.TransactionContext.Inputs;

public sealed class GetAllTransactionsByAccountIdServiceInput
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public TransactionType? TransactionType { get; }
    public IdValueObject? CategoryId { get; }
    public PaymentMethod? PaymentMethod { get; }
    public PaymentStatus? PaymentStatus { get; }
    public DateTime? StartDate { get; }
    public DateTime? EndDate { get; }
    public decimal? MinValue { get; }
    public decimal? MaxValue { get; }
    public string? TextSearch { get; }

    private GetAllTransactionsByAccountIdServiceInput(int pageNumber, int pageSize, TransactionType? transactionType, IdValueObject? categoryId, 
        PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, 
        string? textSearch)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
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

    public static GetAllTransactionsByAccountIdServiceInput Factory(int pageNumber, int pageSize, TransactionType? transactionType, IdValueObject? categoryId,
        PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue,
        string? textSearch)
        => new(pageNumber, pageSize, transactionType, categoryId, paymentMethod, paymentStatus, startDate, endDate, minValue, maxValue, textSearch);
}
