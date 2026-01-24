using tMoney.Application.Services.CardContext.Interfaces;
using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase;

public sealed class CreateTransactionUseCase : IUseCase<CreateTransactionUseCaseInput, CreateTransactionUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICardService _cardService;
    private readonly ICategoryService _categoryService;

    public CreateTransactionUseCase(ITransactionService transactionService, IInstallmentService installmentService, IUnitOfWork unitOfWork, 
        ICardService cardService, ICategoryService categoryService)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
        _unitOfWork = unitOfWork;
        _cardService = cardService;
        _categoryService = categoryService;
    }

    public async Task<CreateTransactionUseCaseOutput> ExecuteUseCaseAsync(CreateTransactionUseCaseInput input, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var card = await _cardService.GetCardByIdServiceAsync(
                cardId: input.CardId,
                accountId: input.AccountId,
                cancellationToken: cancellationToken);
            if (card is null)
                throw new KeyNotFoundException("Cartão não encontrado.");

            if (card.Type == CardType.CreditCard.ToString())
            {
                if (input.TransactionType == TransactionType.Income)
                    throw new ArgumentException("Não é possível criar transações de entrada em cartões de crédito.");

                if (input.PaymentMethod != PaymentMethod.Credit)
                    throw new ArgumentException("Método de pagamento inválido para cartão de crédito.");
            }
            else
            {
                if (input.PaymentMethod == PaymentMethod.Credit)
                    throw new ArgumentException($"O método de pagamento 'Cartão de Crédito' só pode ser utilizado em cartões de crédito.");
            }

            var category = await _categoryService.GetCategoryByIdServiceAsync(input.CategoryId, input.AccountId, cancellationToken);
            if (category is null)
                throw new KeyNotFoundException("Categoria não encontrada");

            if (input.TransactionType == TransactionType.Income && category.Type == CategoryType.Expense.ToString() ||
                input.TransactionType == TransactionType.Expense && category.Type == CategoryType.Income.ToString())
                throw new ArgumentException($"A categoria '{category.Title}' ({category.Type.ToString()}) não pode ser usada para uma transação do tipo {input.TransactionType.ToString()}.");

            CreateInstallmentServiceOutput? installmentServiceOutput = null;
            IdValueObject? firstInvoiceId = null;
            IdValueObject? voInstallmentId = null;
            IdValueObject[]? invoiceIds = null;

            if (input.PaymentMethod == PaymentMethod.Credit)
            {
                var totalInstallments = input.HasInstallment?.TotalInstallments ?? 1;

                var invoiceServiceOutput = await _cardService.CreateInvoiceByCardIdServiceAsync(
                    cardId: input.CardId,
                    accountId: input.AccountId,
                    firstPaymentDate: input.Date,
                    totalInstallments: totalInstallments,
                    cancellationToken: cancellationToken);

                invoiceIds = invoiceServiceOutput.Invoices
                    .OrderBy(i => i.DueDay)
                    .Select(i =>
                    {
                        if (!Guid.TryParse(i.InvoiceId, out var invoiceId))
                            throw new ArgumentException("Invoice ID inválido.");

                        return IdValueObject.Factory(invoiceId);
                    })
                    .ToArray();

                if (input.HasInstallment is null)
                    firstInvoiceId = invoiceIds.First();
            }

            if (input.HasInstallment is not null)
            {
                installmentServiceOutput = await _installmentService.CreateInstallmentServiceAsync(
                    input: CreateInstallmentServiceInput.Factory(
                        accountId: input.AccountId,
                        invoiceIds: invoiceIds,
                        totalInstallments: input.HasInstallment.TotalInstallments,
                        totalAmount: input.Amount,
                        firstPaymentDate: input.Date,
                        status: input.Status),
                    cancellationToken: cancellationToken);

                if (!Guid.TryParse(installmentServiceOutput.Id, out var installmentId))
                    throw new ArgumentException("Installment ID inválido.");

                voInstallmentId = IdValueObject.Factory(installmentId);
            }

            var transactionServiceOutput = await _transactionService.CreateTransactionServiceAsync(
                CreateTransactionServiceInput.Factory(
                    accountId: input.AccountId,
                    cardId: input.CardId,
                    categoryId: input.CategoryId,
                    installmentId: voInstallmentId,
                    invoiceId: firstInvoiceId,
                    title: input.Title,
                    description: input.Description,
                    amount: input.Amount,
                    date: input.Date,
                    transactionType: input.TransactionType,
                    paymentMethod: input.PaymentMethod,
                    status: input.Status,
                    destination: input.Destination),
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var installmentOutput = installmentServiceOutput is null ? null :
                CreateTransactionUseCaseOutputInstallment.Factory(
                        id: installmentServiceOutput.Id,
                        totalInstallments: installmentServiceOutput.TotalInstallments,
                        totalAmount: installmentServiceOutput.TotalAmount,
                        items: installmentServiceOutput.InstallmentItems
                            .Select(item => CreateTransactionUseCaseOutputInstallmentItem.Factory(
                                id: item.Id,
                                invoiceId: item.InvoiceId,
                                number: item.Number,
                                amount: item.Amount,
                                dueDate: item.DueDate,
                                status: item.Status,
                                paidAt: item.PaidAt,
                                updatedAt: item.UpdatedAt,
                                createdAt: item.CreatedAt))
                            .ToArray(),
                        updatedAt: installmentServiceOutput.UpdatedAt,
                        createdAt: installmentServiceOutput.CreatedAt);

            var output = CreateTransactionUseCaseOutput.Factory(
                    id: transactionServiceOutput.Id,
                    accountId: transactionServiceOutput.AccountId,
                    cardId: card.Id,
                    categoryId: transactionServiceOutput.CategoryId,
                    title: transactionServiceOutput.Title,
                    description: transactionServiceOutput.Description,
                    amount: transactionServiceOutput.Amount,
                    date: transactionServiceOutput.Date,
                    transactionType: transactionServiceOutput.TransactionType,
                    paymentMethod: transactionServiceOutput.PaymentMethod,
                    status: transactionServiceOutput.Status,
                    destination: transactionServiceOutput.Destination,
                    updatedAt: transactionServiceOutput.UpdatedAt,
                    createdAt: transactionServiceOutput.CreatedAt,
                    installment: installmentOutput);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
