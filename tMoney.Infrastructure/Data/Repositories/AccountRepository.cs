using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Account?> GetAccountByIdAsync(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out Guid accountIdGuid))
            return null;

        var voAccountId = IdValueObject.Factory(accountIdGuid);

        return await _dataContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == voAccountId, cancellationToken);
    }
}
