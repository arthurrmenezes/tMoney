using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase;

public class GetAllTransactionsUseCase : IUseCase<GetAllTransactionsUseCaseInput, GetAllTransactionsUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;

    public GetAllTransactionsUseCase(ITransactionService transactionService, IInstallmentService installmentService)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
    }

    public async Task<GetAllTransactionsUseCaseOutput> ExecuteUseCaseAsync(GetAllTransactionsUseCaseInput input, CancellationToken cancellationToken)
    {
        var transactionServiceOutput = await _transactionService.GetAllTransactionsByAccountIdServiceAsync(
            accountId: input.AccountId,
            input: GetAllTransactionsByAccountIdServiceInput.Factory(
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                cardId: input.CardId,
                transactionType: input.TransactionType,
                categoryId: input.CategoryId,
                paymentMethod: input.PaymentMethod,
                paymentStatus: input.PaymentStatus,
                startDate: input.StartDate,
                endDate: input.EndDate,
                minValue: input.MinValue,
                maxValue: input.MaxValue,
                textSearch: input.TextSearch,
                hasInstallment: input.HasInstallment),
            cancellationToken: cancellationToken);

        GetAllTransactionsUseCaseOutputInstallment[]? installmentOutput = null;

        if (!input.HasInstallment.HasValue || input.HasInstallment.Value == true)
        {
            var installmentIds = transactionServiceOutput.Transactions
                .Where(t => !string.IsNullOrEmpty(t.InstallmentId))
                .Select(t => IdValueObject.Factory(Guid.Parse(t.InstallmentId!)))
                .ToArray();

            if (installmentIds.Any())
            {
                var installmentServiceOutput = await _installmentService.GetAllInstallmentsByTransactionIdServiceAsync(
                    accountId: input.AccountId,
                    installmentIds: installmentIds,
                    cancellationToken: cancellationToken);

                installmentOutput = installmentServiceOutput.Select(i => new GetAllTransactionsUseCaseOutputInstallment(
                    id: i.Id,
                    totalInstallments: i.TotalInstallments,
                    totalAmount: i.TotalAmount,
                    firstPaymentDate: i.FirstPaymentDate,
                    status: i.Status,
                    installmentItems: i.Installments.Select(ii => GetAllTransactionsUseCaseOutputInstallmentItem.Factory(
                        id: ii.Id,
                        invoiceId: ii.InvoiceId,
                        number: ii.Number,
                        amount: ii.Amount,
                        dueDate: ii.DueDate,
                        status: ii.Status,
                        paidAt: ii.PaidAt,
                        updatedAt: ii.UpdatedAt,
                        createdAt: ii.CreatedAt))
                    .ToArray()))
                .ToArray();
            }
        }

        var installmentDictionary = installmentOutput?.ToDictionary(k => k.Id, v => v) ?? 
            new Dictionary<string, GetAllTransactionsUseCaseOutputInstallment>();

        var mappedTransactions = transactionServiceOutput.Transactions.Select(t =>
        {
            GetAllTransactionsUseCaseOutputInstallment? match = null;

            if (!string.IsNullOrEmpty(t.InstallmentId) && installmentDictionary.TryGetValue(t.InstallmentId, out var found))
                match = found;

            var installmentList = match is not null ? new[] { match } : null;

            return GetAllTransactionsUseCaseOutputTransaction.Factory(
                id: t.Id,
                accountId: t.AccountId,
                cardId: t.CardId,
                categoryId: t.CategoryId,
                installmentId: t.InstallmentId,
                invoiceId: t.InvoiceId,
                title: t.Title,
                description: t.Description,
                amount: t.Amount,
                date: t.Date,
                transactionType: t.TransactionType,
                paymentMethod: t.PaymentMethod,
                status: t.Status,
                destination: t.Destination,
                installments: installmentList!,
                updatedAt: t.UpdatedAt,
                createdAt: t.CreatedAt);
        }).ToArray();

        var output = GetAllTransactionsUseCaseOutput.Factory(
            totalTransactions: transactionServiceOutput.TotalTransactions,
            pageNumber: transactionServiceOutput.PageNumber,
            pageSize: transactionServiceOutput.PageSize,
            totalPages: transactionServiceOutput.TotalPages,
            transactions: mappedTransactions);

        return output;
    }
}
