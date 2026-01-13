using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.TransactionContext.Interfaces;

public interface ITransactionService
{
    public Task<CreateTransactionServiceOutput> CreateTransactionServiceAsync(
        CreateTransactionServiceInput input,
        CancellationToken cancellationToken);

    public Task<GetTransactionByIdServiceOutput> GetTransactionByIdServiceAsync(
        IdValueObject transactionId,
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<GetAllTransactionsByAccountIdServiceOutput> GetAllTransactionsByAccountIdServiceAsync(
        IdValueObject accountId,
        GetAllTransactionsByAccountIdServiceInput input,
        CancellationToken cancellationToken);

    public Task<UpdateTransactionDetailsByIdServiceOutput> UpdateTransactionDetailsByIdServiceAsync(
        IdValueObject transactionId,
        IdValueObject accountId,
        UpdateTransactionDetailsByIdServiceInput input,
        CancellationToken cancellationToken);

    public Task<DeleteTransactionByIdServiceOutput> DeleteTransactionByIdServiceAsync(
        IdValueObject transactionId,
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<GetFinancialSummaryServiceOutput> GetFinancialSummaryServiceAsync(
        IdValueObject accountId,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken);
}
