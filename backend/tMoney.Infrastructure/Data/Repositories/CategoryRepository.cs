using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Category?> GetByIdAsync(Guid categoryId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dataContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == IdValueObject.Factory(categoryId) && c.AccountId == IdValueObject.Factory(accountId), cancellationToken);
    }

    public async Task<Category?> GetByTitleAsync(string title, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dataContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Title == title && c.AccountId == IdValueObject.Factory(accountId), cancellationToken);
    }

    public async Task<Category[]> GetAllAsync(Guid accountId, int pageNumber, int pageSize, CategoryType? categoryType, 
        CancellationToken cancellationToken)
    {
        int skip = (pageNumber - 1) * pageSize;

        var voAccountId = IdValueObject.Factory(accountId);

        var query = _dataContext.Categories
            .AsNoTracking()
            .Where(c => c.AccountId == voAccountId);

        if (categoryType.HasValue)
        {
            if (categoryType.Value == CategoryType.Both)
                query = query.Where(c => c.Type == CategoryType.Both);
            else
                query = query.Where(c => c.Type == categoryType.Value || c.Type == CategoryType.Both);
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetTotalCategoriesNumberAsync(Guid accountId, CategoryType? categoryType, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        var query = _dataContext.Categories
            .AsNoTracking()
            .Where(c => c.AccountId == voAccountId);

        if (categoryType.HasValue)
        {
            if (categoryType.Value == CategoryType.Both)
                query = query.Where(c => c.Type == CategoryType.Both);
            else
                query = query.Where(c => c.Type == categoryType.Value || c.Type == CategoryType.Both);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<Category> GetDefaultCategoryByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var defaultCategory = await _dataContext.Categories
            .FirstOrDefaultAsync(c => c.IsDefault && c.AccountId == IdValueObject.Factory(accountId), cancellationToken);

        return defaultCategory!;
    }
}
