using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(id);

        return await _dataContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AccountId == voAccountId, cancellationToken);
    }

    public async Task<Account?> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dataContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }
}
