using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase;

public sealed class CreateTransactionUseCase : IUseCase<CreateTransactionUseCaseInput, CreateTransactionUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionUseCase(ITransactionService transactionService, IInstallmentService installmentService, IUnitOfWork unitOfWork)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTransactionUseCaseOutput> ExecuteUseCaseAsync(CreateTransactionUseCaseInput input, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            CreateInstallmentServiceOutput? installmentServiceOutput = null;
            IdValueObject? voInstallmentId = null;

            if (input.HasInstallment is not null)
            {
                installmentServiceOutput = await _installmentService.CreateInstallmentServiceAsync(
                    input: CreateInstallmentServiceInput.Factory(
                        accountId: input.AccountId,
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
                new CreateTransactionUseCaseOutputInstallment(
                        id: installmentServiceOutput.Id.ToString(),
                        totalInstallments: installmentServiceOutput.TotalInstallments,
                        totalAmount: installmentServiceOutput.TotalAmount,
                        items: installmentServiceOutput.InstallmentItems
                            .Select(item => new CreateTransactionUseCaseOutputInstallmentItem(
                                id: item.Id.ToString(),
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
