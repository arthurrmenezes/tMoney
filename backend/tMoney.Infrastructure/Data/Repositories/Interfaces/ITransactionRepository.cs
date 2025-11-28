using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    public Task UpdateCategoryForDefaultAsync(Guid currentCategoryId, Guid defaultCategoryId, Guid accountId, CancellationToken cancellationToken);
}
