using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DataContext dataContext) : base(dataContext) { }

    public async Task UpdateCategoryForDefaultAsync(Guid currentCategoryId, Guid defaultCategoryId, Guid accountId, CancellationToken cancellationToken)
    {
        await _dataContext.Transactions
            .Where(t => t.CategoryId == IdValueObject.Factory(currentCategoryId) && t.AccountId == IdValueObject.Factory(accountId))
            .ExecuteUpdateAsync(calls => 
                calls.SetProperty(t => t.CategoryId, IdValueObject.Factory(defaultCategoryId)), cancellationToken);
    }
}
