using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CardRepository : BaseRepository<Card>, ICardRepository
{
    public CardRepository(DataContext dataContext) : base(dataContext) { }
}
