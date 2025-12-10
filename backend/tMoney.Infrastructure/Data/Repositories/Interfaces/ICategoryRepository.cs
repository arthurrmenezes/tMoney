using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<Category?> GetByIdAsync(Guid categoryId, Guid accountId, CancellationToken cancellationToken);
    public Task<Category?> GetByTitleAsync(string title, Guid accountId, CancellationToken cancellationToken);
    public Task<Category[]> GetAllAsync(Guid accountId, int pageNumber, int pageSize, CategoryType? categoryType, CancellationToken cancellationToken);
    public Task<int> GetTotalCategoriesNumberAsync(Guid accountId, CategoryType? categoryType, CancellationToken cancellationToken);
    public Task<Category> GetDefaultCategoryByIdAsync(Guid accountId, CancellationToken cancellationToken);
}
