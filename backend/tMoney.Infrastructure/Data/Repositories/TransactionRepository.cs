using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DataContext dataContext) : base(dataContext) { }

    public async Task UpdateCategoryForDefaultAsync(Guid currentCategoryId, Guid defaultCategoryId, Guid accountId, CancellationToken cancellationToken)
    {
        var voDefaultCategoryId = IdValueObject.Factory(defaultCategoryId);

        await _dataContext.Transactions
            .Where(t => t.CategoryId == IdValueObject.Factory(currentCategoryId) && t.AccountId == IdValueObject.Factory(accountId))
            .ExecuteUpdateAsync(calls =>
                calls.SetProperty(t => t.CategoryId, voDefaultCategoryId), cancellationToken);
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken)
    {
        var voTransactionId = IdValueObject.Factory(transactionId);
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == voTransactionId && t.AccountId == voAccountId, cancellationToken);
    }

    public async Task<Transaction[]> GetAllByAccountIdAsync(Guid accountId, int pageNumber, int pageSize, TransactionType? transactionType, Guid? categoryId,
        PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue,
        string? textSearch, bool? hasInstallment, CancellationToken cancellationToken)
    {
        var skip = (pageNumber - 1) * pageSize;
        var voAccountId = IdValueObject.Factory(accountId);

        if (startDate is not null && startDate.Value.Kind == DateTimeKind.Unspecified)
            startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);

        if (endDate is not null && endDate.Value.Kind == DateTimeKind.Unspecified)
            endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);

        var query = _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId);

        if (transactionType.HasValue)
            query = query.Where(t => t.TransactionType == transactionType);

        if (categoryId.HasValue)
        {
            var voCategoryId = IdValueObject.Factory(categoryId.Value);

            query = query.Where(t => t.CategoryId == voCategoryId);
        }

        if (paymentMethod.HasValue)
            query = query.Where(t => t.PaymentMethod == paymentMethod);

        if (paymentStatus.HasValue)
            query = query.Where(t => t.Status == paymentStatus);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.Date >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.Date <= endDate);
        }

        if (minValue.HasValue)
            query = query.Where(t => t.Amount >= minValue);

        if (maxValue.HasValue)
            query = query.Where(t => t.Amount <= maxValue);

        if (!string.IsNullOrEmpty(textSearch))
        {
            var text = textSearch.ToLower();

            query = query.Where(t =>
                (t.Title != null && t.Title.ToLower().Contains(text)) ||
                (t.Description != null && t.Description.ToLower().Contains(text)) ||
                (t.Destination != null && t.Destination.ToLower().Contains(text)));
        }

        if (hasInstallment.HasValue)
            if (hasInstallment.Value)
                query = query.Where(t => t.InstallmentId != null);
            else
                query = query.Where(t => t.InstallmentId == null);

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetTransactionsCountAsync(Guid accountId, TransactionType? transactionType, Guid? categoryId, PaymentMethod? paymentMethod,
        PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, string? textSearch, bool? hasInstallment,
        CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        if (startDate is not null && startDate.Value.Kind == DateTimeKind.Unspecified)
            startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);

        if (endDate is not null && endDate.Value.Kind == DateTimeKind.Unspecified)
            endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);

        var query = _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId);

        if (transactionType.HasValue)
            query = query.Where(t => t.TransactionType == transactionType);

        if (categoryId.HasValue)
        {
            var voCategoryId = IdValueObject.Factory(categoryId.Value);
            query = query.Where(t => t.CategoryId == voCategoryId);
        }

        if (paymentMethod.HasValue)
            query = query.Where(t => t.PaymentMethod == paymentMethod);

        if (paymentStatus.HasValue)
            query = query.Where(t => t.Status == paymentStatus);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate);

        if (minValue.HasValue)
            query = query.Where(t => t.Amount >= minValue);

        if (maxValue.HasValue)
            query = query.Where(t => t.Amount <= maxValue);

        if (!string.IsNullOrEmpty(textSearch))
        {
            var text = textSearch.ToLower();

            query = query.Where(t =>
                (t.Title != null && t.Title.ToLower().Contains(text)) ||
                (t.Description != null && t.Description.ToLower().Contains(text)) ||
                (t.Destination != null && t.Destination.ToLower().Contains(text)));
        }

        if (hasInstallment.HasValue)
            if (hasInstallment.Value)
                query = query.Where(t => t.InstallmentId != null);
            else
                query = query.Where(t => t.InstallmentId == null);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<(decimal totalIncome, decimal totalExpense)> GetFinancialSummaryAsync(Guid accountId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken)
    {
        if (startDate.Kind == DateTimeKind.Unspecified)
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);

        if (endDate.Kind == DateTimeKind.Unspecified)
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

        var voAccountId = IdValueObject.Factory(accountId);

        var query = await _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId &&
                t.Date >= startDate &&
                t.Date <= endDate &&
                t.Status == PaymentStatus.Paid)
            .GroupBy(t => 1)
            .Select(g => new
            {
                Income = g.Where(t => t.TransactionType == TransactionType.Income)
                      .Sum(t => t.Amount),
                Expense = g.Where(t => t.TransactionType == TransactionType.Expense)
                       .Sum(t => t.Amount)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return (query?.Income ?? 0, query?.Expense ?? 0);
    }

    public async Task<decimal> GetTotalBalanceAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var voAccountId = IdValueObject.Factory(accountId);

        return await _dataContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == voAccountId && t.Status == PaymentStatus.Paid)
            .SumAsync(t => t.TransactionType == TransactionType.Income 
                ? t.Amount
                : t.TransactionType == TransactionType.Expense
                    ? -t.Amount
                    : 0,
            cancellationToken);
    }

    public async Task UpdateOverdueTransactionsAsync(CancellationToken cancellationToken)
    {
        await _dataContext.Transactions
            .Where(t => t.Status == PaymentStatus.Pending && DateTime.UtcNow > t.Date)
            .ExecuteUpdateAsync(calls =>
                calls.SetProperty(t => t.Status, PaymentStatus.Overdue)
                    .SetProperty(t => t.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }
}
