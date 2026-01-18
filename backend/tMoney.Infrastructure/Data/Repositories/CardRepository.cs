using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CardRepository : BaseRepository<Card>, ICardRepository
{
    public CardRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Card?> GetByIdAsync(Guid cardId, Guid accountId, CancellationToken cancellationToken)
    {
        var voCardId = IdValueObject.Factory(cardId);
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Cards
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == voCardId && c.AccountId == voAccountId, cancellationToken);
    }
}
