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
        var voDefaultCategoryId = IdValueObject.Factory(defaultCategoryId);

        await _dataContext.Transactions
            .Where(t => t.CategoryId == IdValueObject.Factory(currentCategoryId) && t.AccountId == IdValueObject.Factory(accountId))
            .ExecuteUpdateAsync(calls => 
                calls.SetProperty(t => t.CategoryId, voDefaultCategoryId), cancellationToken);
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken)
    {
        var voTransactionId = IdValueObject.Factory(transactionId);
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == voTransactionId && t.AccountId == voAccountId, cancellationToken);
    }

    public async Task<Transaction[]> GetAllByAccountIdAsync(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var skip = (pageNumber - 1) * pageSize;
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetTransactionsCountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId)
            .CountAsync(cancellationToken);
    }
}
