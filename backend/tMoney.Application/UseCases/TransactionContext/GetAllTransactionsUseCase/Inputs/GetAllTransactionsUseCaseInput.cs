using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Inputs;

public sealed class GetAllTransactionsUseCaseInput
{
    public IdValueObject AccountId { get; }
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
    public bool? HasInstallment { get; }

    private GetAllTransactionsUseCaseInput(IdValueObject accountId, int pageNumber, int pageSize, TransactionType? transactionType, IdValueObject? categoryId, 
        PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, 
        string? textSearch, bool? hasInstallment)
    {
        AccountId = accountId;
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
        HasInstallment = hasInstallment;
    }

    public static GetAllTransactionsUseCaseInput Factory(IdValueObject accountId, int pageNumber, int pageSize, TransactionType? transactionType,
        IdValueObject? categoryId, PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue,
        decimal? maxValue, string? textSearch, bool? hasInstallment)
        => new(accountId, pageNumber, pageSize, transactionType, categoryId, paymentMethod, paymentStatus, startDate, endDate, minValue, maxValue, textSearch,
            hasInstallment);
}
