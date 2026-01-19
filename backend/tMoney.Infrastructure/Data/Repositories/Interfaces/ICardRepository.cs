using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICardRepository : IBaseRepository<Card>
{
    public Task<Card?> GetByIdAsync(Guid cardId, Guid accountId, bool toUpdate, CancellationToken cancellationToken);

    public Task<Card[]> GetAllByAccountId(Guid accountId, int pageNumber, int pageSize, CardType? cardType, CancellationToken cancellationToken);

    public Task<int> GetTotalCardsNumberAsync(Guid accountId, CancellationToken cancellationToken);
}
