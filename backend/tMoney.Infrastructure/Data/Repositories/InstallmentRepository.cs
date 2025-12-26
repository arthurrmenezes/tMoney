using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class InstallmentRepository : BaseRepository<Installment>, IInstallmentRepository
{
    public InstallmentRepository(DataContext dataContext) : base(dataContext) { }
}
