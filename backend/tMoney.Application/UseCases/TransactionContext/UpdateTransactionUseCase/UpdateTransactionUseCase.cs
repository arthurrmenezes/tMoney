using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Outputs;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase;

public sealed class UpdateTransactionUseCase : IUseCase<UpdateTransactionUseCaseInput, UpdateTransactionUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryService _categoryService;

    public UpdateTransactionUseCase(ITransactionService transactionService, IUnitOfWork unitOfWork, ICategoryService categoryService)
    {
        _transactionService = transactionService;
        _unitOfWork = unitOfWork;
        _categoryService = categoryService;
    }

    public async Task<UpdateTransactionUseCaseOutput> ExecuteUseCaseAsync(
        UpdateTransactionUseCaseInput input, 
        CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var transaction = await _transactionService.GetTransactionByIdServiceAsync(
                transactionId: input.TransactionId,
                accountId: input.AccountId,
                cancellationToken: cancellationToken);

            if (input.Amount.HasValue && input.Amount.Value != transaction.Amount)
                if (transaction.InstallmentId is not null)
                    throw new InvalidOperationException("Não é possível editar o valor de uma transação parcelada.");

            if (input.CategoryId is not null)
            {
                var category = await _categoryService.GetCategoryByIdServiceAsync(
                    categoryId: input.CategoryId,
                    accountId: input.AccountId,
                    cancellationToken: cancellationToken);

                if (transaction.TransactionType == TransactionType.Income.ToString() && category.Type == CategoryType.Expense.ToString() ||
                    transaction.TransactionType == TransactionType.Expense.ToString() && category.Type == CategoryType.Income.ToString())
                    throw new ArgumentException($"A categoria '{category.Title}' ({category.Type.ToString()}) não pode ser usada para uma transação do tipo {transaction.TransactionType.ToString()}.");
            }

            var transactionServiceOutput = await _transactionService.UpdateTransactionDetailsByIdServiceAsync(
                transactionId: input.TransactionId,
                accountId: input.AccountId,
                input: UpdateTransactionDetailsByIdServiceInput.Factory(
                    categoryId: input.CategoryId,
                    title: input.Title,
                    description: input.Description,
                    amount: input.Amount,
                    date: input.Date,
                    status: input.Status,
                    destination: input.Destination),
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = UpdateTransactionUseCaseOutput.Factory(
                id: transactionServiceOutput.Id,
                accountId: transactionServiceOutput.AccountId,
                cardId: transactionServiceOutput.CardId,
                categoryId: transactionServiceOutput.CategoryId,
                installmentId: transactionServiceOutput.InstallmentId,
                invoiceId: transactionServiceOutput.InvoiceId,
                title: transactionServiceOutput.Title,
                description: transactionServiceOutput.Description,
                amount: transactionServiceOutput.Amount,
                date: transactionServiceOutput.Date,
                transactionType: transactionServiceOutput.TransactionType,
                paymentMethod: transactionServiceOutput.PaymentMethod,
                status: transactionServiceOutput.Status,
                destination: transactionServiceOutput.Destination,
                updatedAt: transactionServiceOutput.UpdatedAt,
                createdAt: transactionServiceOutput.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
