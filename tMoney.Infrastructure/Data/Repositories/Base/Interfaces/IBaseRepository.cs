namespace tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

public interface IBaseRepository<T> where T : class
{
    public Task AddAsync(T entity, CancellationToken cancellationToken);
    public void Delete(T entity);
}
