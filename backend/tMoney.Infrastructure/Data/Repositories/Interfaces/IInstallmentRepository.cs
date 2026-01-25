using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Infrastructure.Data.DTOs;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface IInstallmentRepository : IBaseRepository<Installment>
{
    public Task<Installment?> GetByIdAsync(Guid installmentId, Guid accountId, bool toUpdate, CancellationToken cancellationToken);

    public Task<Installment[]> GetAllByInstallmentIdAsync(Guid accountId, IEnumerable<Guid> installmentIds, CancellationToken cancellationToken);

    public Task UpdateOverdueInstallmentsAsync(CancellationToken cancellationToken);

    public Task<GetItemsByInvoiceIdDto[]> GetItemsByInvoiceIdAsync(Guid invoiceId, CancellationToken cancellationToken);
}
