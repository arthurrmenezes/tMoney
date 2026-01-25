using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    public Task UpdateCategoryForDefaultAsync(Guid currentCategoryId, Guid defaultCategoryId, Guid accountId, CancellationToken cancellationToken);

    public Task<Transaction?> GetByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken);

    public Task<Transaction[]> GetAllByAccountIdAsync(Guid accountId, int pageNumber, int pageSize, Guid? cardId, TransactionType? transactionType, 
        Guid? categoryId, PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, 
        decimal? maxValue, string? textSearch, bool? hasInstallment, CancellationToken cancellationToken);

    public Task<int> GetTransactionsCountAsync(Guid accountId, Guid? cardId, TransactionType? transactionType, Guid? categoryId, 
        PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DateTime? startDate, DateTime? endDate, decimal? minValue, decimal? maxValue, 
        string? textSearch, bool? hasInstallment, CancellationToken cancellationToken);
    
    public Task<(decimal totalIncome, decimal totalExpense)> GetFinancialSummaryAsync(Guid accountId, Guid cardId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken);

    public Task<decimal> GetTotalBalanceAsync(Guid accountId, Guid cardId, CancellationToken cancellationToken);

    public Task UpdateOverdueTransactionsAsync(CancellationToken cancellationToken);

    public Task<Transaction[]> GetAllByInvoiceId(Guid accountId, Guid cardId, Guid invoiceId, CancellationToken cancellationToken);
}
