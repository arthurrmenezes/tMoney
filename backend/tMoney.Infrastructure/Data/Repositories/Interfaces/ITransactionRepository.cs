using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    public Task UpdateCategoryForDefaultAsync(Guid currentCategoryId, Guid defaultCategoryId, Guid accountId, CancellationToken cancellationToken);
    public Task<Transaction?> GetByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken);
    public Task<Transaction[]> GetAllByAccountIdAsync(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    public Task<int> GetTransactionsCountAsync(Guid accountId, CancellationToken cancellationToken);
}
