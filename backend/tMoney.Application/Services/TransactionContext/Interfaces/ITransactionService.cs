using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Outputs;

namespace tMoney.Application.Services.TransactionContext.Interfaces;

public interface ITransactionService
{
    public Task<CreateTransactionServiceOutput> CreateTransactionServiceAsync(
        CreateTransactionServiceInput input,
        CancellationToken cancellationToken);
}
