using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface IInstallmentRepository : IBaseRepository<Installment>
{
    public Task<Installment?> GetByIdAsync(Guid installmentId, Guid accountId, CancellationToken cancellationToken);
}
