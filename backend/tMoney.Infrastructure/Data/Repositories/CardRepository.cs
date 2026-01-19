using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CardRepository : BaseRepository<Card>, ICardRepository
{
    public CardRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Card?> GetByIdAsync(Guid cardId, Guid accountId, bool toUpdate, CancellationToken cancellationToken)
    {
        var voCardId = IdValueObject.Factory(cardId);
        var voAccountId = IdValueObject.Factory(accountId);

        IQueryable<Card> query = _dataContext.Cards;

        if (!toUpdate)
            query = query.AsNoTracking();

        return await query
            .FirstOrDefaultAsync(c => c.Id == voCardId && c.AccountId == voAccountId, cancellationToken);
    }

    public async Task<Card[]> GetAllByAccountId(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var skip = (pageNumber - 1) * pageSize;

        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Cards
            .AsNoTracking()
            .Where(c => c.AccountId == voAccountId)
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetTotalCardsNumberAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Cards
            .AsNoTracking()
            .Where(c => c.AccountId == voAccountId)
            .CountAsync(cancellationToken);
    }
}
