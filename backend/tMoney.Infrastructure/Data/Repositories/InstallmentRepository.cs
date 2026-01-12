using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class InstallmentRepository : BaseRepository<Installment>, IInstallmentRepository
{
    public InstallmentRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<Installment?> GetByIdAsync(Guid installmentId, Guid accountId, bool toUpdate, CancellationToken cancellationToken)
    {
        var voInstallmentId = IdValueObject.Factory(installmentId);
        var voAccountId = IdValueObject.Factory(accountId);

        IQueryable<Installment> query = _dataContext.Installments;

        if (!toUpdate)
            query = query.AsNoTracking();

        return await query
            .Include(i => i.Installments)
            .FirstOrDefaultAsync(i => i.Id == voInstallmentId && i.AccountId == voAccountId, cancellationToken);
    }

    public async Task<Installment[]> GetAllByInstallmentIdAsync(Guid accountId, IEnumerable<Guid> installmentIds, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);
        var voInstallmentIds = installmentIds
            .Select(id => IdValueObject.Factory(id))
            .ToArray();

        return await _dataContext.Installments
            .AsNoTracking()
            .Include(i => i.Installments)
            .Where(i => i.AccountId == voAccountId && voInstallmentIds.Contains(i.Id))
            .ToArrayAsync(cancellationToken);
    }
}
