using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.DTOs;
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
    
    public async Task<CreditCardInvoiceDto[]> GetAllByCardIdAsync(Guid creditCardId, int pageNumber, int pageSize, 
        CancellationToken cancellationToken)
    {
        var skip = (pageNumber - 1) * pageSize;
        var voCardId = IdValueObject.Factory(creditCardId);

        return await _dataContext.CreditCardInvoices
            .AsNoTracking()
            .Where(c => c.CreditCardId == voCardId)
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .Select(i => new CreditCardInvoiceDto(
                i.Id.ToString(),
                i.CreditCardId.ToString(),
                i.Month,
                i.Year,
                i.CloseDay,
                i.DueDay,
                _dataContext.Transactions.Where(t => t.InvoiceId == i.Id).Sum(t => t.Amount) +
                              _dataContext.InstallmentItems.Where(it => it.InvoiceId == i.Id).Sum(it => it.Amount),
                i.LimitTotal,
                i.AmountPaid,
                i.Status,
                i.UpdatedAt,
                i.CreatedAt
            ))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetTotalInvoicesNumberAsync(Guid creditCardId, CancellationToken cancellationToken)
    {
        var voCardId = IdValueObject.Factory(creditCardId);

        return await _dataContext.CreditCardInvoices
            .AsNoTracking()
            .Where(c => c.CreditCardId == voCardId)
            .CountAsync(cancellationToken);
    }
}
