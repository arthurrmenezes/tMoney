using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ICreditCardInvoiceRepository : IBaseRepository<CreditCardInvoice>
{
    public Task<CreditCardInvoice?> GetOpenInvoiceAsync(Guid creditCardId, CancellationToken cancellationToken);
}
