using tMoney.Application.Services.CardContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsByInvoiceIdUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsByInvoiceIdUseCase.Outputs;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;

namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsByInvoiceIdUseCase;

public sealed class GetAllTransactionsByInvoiceIdUseCase : IUseCase<GetAllTransactionsByInvoiceIdUseCaseInput, GetAllTransactionsByInvoiceIdUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;
    private readonly ICardService _cardService;

    public GetAllTransactionsByInvoiceIdUseCase(ITransactionService transactionService, IInstallmentService installmentService, ICardService cardService)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
        _cardService = cardService;
    }

    public async Task<GetAllTransactionsByInvoiceIdUseCaseOutput> ExecuteUseCaseAsync(
        GetAllTransactionsByInvoiceIdUseCaseInput input, 
        CancellationToken cancellationToken)
    {
        var card = await _cardService.GetCardByIdServiceAsync(
            cardId: input.CardId,
            accountId: input.AccountId,
            cancellationToken: cancellationToken);

        if (card.Type != CardType.CreditCard.ToString())
            throw new ArgumentException($"Somente cartões de crédito possuem faturas.");

        var transactionServiceOutput = await _transactionService.GetAllTransactionsByInvoiceIdServiceAsync(
            accountId: input.AccountId,
            cardId: input.CardId,
            invoiceId: input.InvoiceId,
            cancellationToken: cancellationToken);

        var installmentServiceOutput = await _installmentService.GetAllInstallmentItemsByInvoiceIdServiceAsync(
            invoiceId: input.InvoiceId,
            cancellationToken: cancellationToken);

        var finalTransactionList = new List<GetAllTransactionsByInvoiceIdUseCaseOutputTransaction>();

        foreach (var transaction in transactionServiceOutput)
        {
            finalTransactionList.Add(GetAllTransactionsByInvoiceIdUseCaseOutputTransaction.Factory(
                id: transaction.Id,
                accountId: transaction.AccountId,
                cardId: transaction.CardId,
                categoryId: transaction.CategoryId,
                installmentId: transaction.InstallmentId,
                invoiceId: transaction.InvoiceId,
                title: transaction.Title,
                description: transaction.Description,
                amount: transaction.Amount,
                date: transaction.Date,
                dueDate: transaction.Date,
                currentNumber: 1,
                totalInstallments: 1,
                transactionType: transaction.TransactionType,
                paymentMethod: transaction.PaymentMethod,
                status: transaction.Status,
                destination: transaction.Destination,
                paidAt: null,
                updatedAt: transaction.UpdatedAt,
                createdAt: transaction.CreatedAt
            ));
        }

        if (installmentServiceOutput is not null)
            foreach (var installmentItem in installmentServiceOutput)
            {
                finalTransactionList.Add(GetAllTransactionsByInvoiceIdUseCaseOutputTransaction.Factory(
                    id: installmentItem.Id,
                    accountId: input.AccountId.ToString(),
                    cardId: input.CardId.ToString(),
                    categoryId: installmentItem.CategoryId,
                    installmentId: installmentItem.InstallmentId,
                    invoiceId: installmentItem.InvoiceId,
                    title: installmentItem.Title,
                    description: $"Parcela {installmentItem.Number}/{installmentItem.TotalInstallments}",
                    amount: installmentItem.Amount,
                    date: installmentItem.DueDate,
                    dueDate: installmentItem.DueDate,
                    currentNumber: installmentItem.Number,
                    totalInstallments: installmentItem.TotalInstallments,
                    transactionType: "Expense",
                    paymentMethod: "Credit",
                    status: installmentItem.Status,
                    destination: null,
                    paidAt: installmentItem.PaidAt,
                    updatedAt: installmentItem.UpdatedAt,
                    createdAt: installmentItem.CreatedAt
                ));
            }

        var sortedTransactionList = finalTransactionList
            .OrderByDescending(t => t.Date)
            .ToList();

        var totalTransactions = sortedTransactionList.Count;
        var totalPages = (int)Math.Ceiling((double)totalTransactions / input.PageSize);

        var pagedTransactions = sortedTransactionList
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToArray();

        var output = GetAllTransactionsByInvoiceIdUseCaseOutput.Factory(
            totalTransactions: totalTransactions,
            pageNumber: input.PageNumber,
            pageSize: input.PageSize,
            totalPages: totalPages,
            transactions: pagedTransactions);

        return output;
    }
}
