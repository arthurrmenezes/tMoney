namespace tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    public Task BeginTransactionAsync(CancellationToken cancellationToken);
    public Task CommitTransactionAsync(CancellationToken cancellationToken);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
