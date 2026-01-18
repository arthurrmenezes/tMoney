using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICardRepository : IBaseRepository<Card>
{
    public Task<Card?> GetByIdAsync(Guid cardId, Guid accountId, CancellationToken cancellationToken);
}
