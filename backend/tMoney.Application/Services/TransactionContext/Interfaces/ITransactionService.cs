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
}
