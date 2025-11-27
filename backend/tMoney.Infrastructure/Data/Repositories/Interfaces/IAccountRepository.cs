using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface IAccountRepository : IBaseRepository<Account>
{
    public Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);
}
