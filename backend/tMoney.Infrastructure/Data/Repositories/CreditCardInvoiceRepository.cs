using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class CreditCardInvoiceRepository : BaseRepository<CreditCardInvoice>, ICreditCardInvoiceRepository
{
    public CreditCardInvoiceRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<CreditCardInvoice?> GetOpenInvoiceAsync(Guid creditCardId, CancellationToken cancellationToken)
    {
        var voCreditCardId = IdValueObject.Factory(creditCardId);

        return await _dataContext.CreditCardInvoices
            .Where(c => c.CreditCardId == voCreditCardId && c.Status == InvoiceStatus.Open)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CreditCardInvoice?> GetByDateAsync(Guid creditCardId, int year, int month, CancellationToken cancellationToken)
    {
        var voCreditCardId = IdValueObject.Factory(creditCardId);

        return await _dataContext.CreditCardInvoices
            .Where(c => c.CreditCardId == voCreditCardId && 
                c.Year == year && 
                c.Month == month)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
