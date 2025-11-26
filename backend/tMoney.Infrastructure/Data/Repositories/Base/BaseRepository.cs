using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DataContext _dataContext;

    public BaseRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dataContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dataContext.Update(entity);
    }

    public void Delete(T entity)
    {
        _dataContext.Set<T>().Remove(entity);
    }
}
