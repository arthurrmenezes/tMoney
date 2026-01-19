using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICardRepository : IBaseRepository<Card>
{
    public Task<Card?> GetByIdAsync(Guid cardId, Guid accountId, bool toUpdate, CancellationToken cancellationToken);

    public Task<Card[]> GetAllByAccountId(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    public Task<int> GetTotalCardsNumberAsync(Guid accountId, CancellationToken cancellationToken);
}
