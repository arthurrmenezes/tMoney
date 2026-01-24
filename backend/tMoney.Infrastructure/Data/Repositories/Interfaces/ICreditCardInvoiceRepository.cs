using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Infrastructure.Data.DTOs;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICreditCardInvoiceRepository : IBaseRepository<CreditCardInvoice>
{
    public Task<CreditCardInvoice?> GetOpenInvoiceAsync(Guid creditCardId, CancellationToken cancellationToken);

    public Task<CreditCardInvoice?> GetByDateAsync(Guid creditCardId, int year, int month, CancellationToken cancellationToken);

    public Task<CreditCardInvoiceDto[]> GetAllByCardIdAsync(Guid creditCardId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    public Task<int> GetTotalInvoicesNumberAsync(Guid creditCardId, CancellationToken cancellationToken);
}
