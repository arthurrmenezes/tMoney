using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class InstallmentRepository : BaseRepository<Installment>, IInstallmentRepository
{
    public InstallmentRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Installment?> GetByIdAsync(Guid installmentId, Guid accountId, CancellationToken cancellationToken)
    {
        var voInstallmentId = IdValueObject.Factory(installmentId);
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Installments
            .AsNoTracking()
            .Include(i => i.Installments)
            .FirstOrDefaultAsync(i => i.Id == voInstallmentId && i.AccountId == voAccountId, cancellationToken);
    }
}
