using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<Category?> GetByTitleAsync(string title, Guid accountId, CancellationToken cancellationToken);
}
