using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Category?> GetByTitleAsync(string title, Guid accountId, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Title == title && c.AccountId == voAccountId, cancellationToken);
    }
}
