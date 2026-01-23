using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Outputs;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase;

public sealed class UpdateTransactionUseCase : IUseCase<UpdateTransactionUseCaseInput, UpdateTransactionUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionUseCase(ITransactionService transactionService, IInstallmentService installmentService, IUnitOfWork unitOfWork)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
        _unitOfWork = unitOfWork;
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

            UpdateInstallmentDetailsByIdServiceOutput? installmentServiceOutput = null;
            string? installmentId = transaction.InstallmentId;

            bool updateInstallment = input.Installment is not null || input.Amount.HasValue || input.Date.HasValue || input.Status.HasValue;

            if (updateInstallment)
            {
                var amount = input.Amount ?? transaction.Amount;
                var date = input.Date ?? transaction.Date;

                if (installmentId is null && input.Installment is not null)
                {
                    if (!Enum.TryParse<PaymentStatus>(
                        value: transaction.Status,
                        ignoreCase: true,
                        result: out var status))
                        throw new ArgumentException($"Status do pagamento inválido: {transaction.Status}");

                    var createInstallmentServiceOutput = await _installmentService.CreateInstallmentServiceAsync(
                        input: CreateInstallmentServiceInput.Factory(
                            accountId: input.AccountId,
                            totalInstallments: input.Installment.TotalInstallments!.Value,
                            totalAmount: amount,
                            firstPaymentDate: date,
                            status: status),
                        cancellationToken: cancellationToken);

                    installmentId = createInstallmentServiceOutput.Id;

                    var installmentItemsOutput = createInstallmentServiceOutput.InstallmentItems
                        .Select(i => new UpdateInstallmentDetailsByIdServiceOutputInstallment(
                            id: i.Id,
                            number: i.Number,
                            amount: i.Amount,
                            dueDate: i.DueDate,
                            status: i.Status,
                            paidAt: i.PaidAt,
                            updatedAt: i.UpdatedAt,
                            createdAt: i.CreatedAt))
                        .ToArray();

                    installmentServiceOutput = UpdateInstallmentDetailsByIdServiceOutput.Factory(
                        id: createInstallmentServiceOutput!.Id,
                        accountId: createInstallmentServiceOutput.AccountId,
                        totalInstallments: createInstallmentServiceOutput.TotalInstallments,
                        totalAmount: createInstallmentServiceOutput.TotalAmount,
                        firstPaymentDate: createInstallmentServiceOutput.FirstPaymentDate,
                        status: createInstallmentServiceOutput.Status,
                        installments: installmentItemsOutput,
                        updatedAt: DateTime.UtcNow,
                        createdAt: createInstallmentServiceOutput.CreatedAt);
                }
                else if (installmentId is not null)
                {
                    if (!Guid.TryParse(installmentId, out var guidInstallmentId))
                        throw new ArgumentException("Installment ID inválido.");

                    var updateInstallmentServiceOutput = await _installmentService.UpdateInstallmentDetailsByIdServiceAsync(
                        installmentId: IdValueObject.Factory(guidInstallmentId),
                        accountId: input.AccountId,
                        input: UpdateInstallmentDetailsByIdServiceInput.Factory(
                            totalInstallments: input.Installment?.TotalInstallments,
                            totalAmount: amount,
                            firstPaymentDate: date,
                            status: input.Status),
                        cancellationToken: cancellationToken);

                    installmentServiceOutput = UpdateInstallmentDetailsByIdServiceOutput.Factory(
                        id: updateInstallmentServiceOutput.Id,
                        accountId: updateInstallmentServiceOutput.AccountId,
                        totalInstallments: updateInstallmentServiceOutput.TotalInstallments,
                        totalAmount: updateInstallmentServiceOutput.TotalAmount,
                        firstPaymentDate: updateInstallmentServiceOutput.FirstPaymentDate,
                        status: updateInstallmentServiceOutput.Status,
                        installments: updateInstallmentServiceOutput.Installments.Select(i => new UpdateInstallmentDetailsByIdServiceOutputInstallment(
                            id: i.Id,
                            number: i.Number,
                            amount: i.Amount,
                            dueDate: i.DueDate,
                            status: i.Status,
                            paidAt: i.PaidAt,
                            updatedAt: i.UpdatedAt,
                            createdAt: i.CreatedAt))
                        .ToArray(),
                        updatedAt: updateInstallmentServiceOutput.UpdatedAt,
                        createdAt: updateInstallmentServiceOutput.CreatedAt);
                }
            }

            var transactionServiceOutput = await _transactionService.UpdateTransactionDetailsByIdServiceAsync(
                transactionId: input.TransactionId,
                accountId: input.AccountId,
                input: UpdateTransactionDetailsByIdServiceInput.Factory(
                    categoryId: input.CategoryId,
                    installmentId: installmentId is null ? null : IdValueObject.Factory(Guid.Parse(installmentId)),
                    title: input.Title,
                    description: input.Description,
                    amount: input.Amount,
                    date: input.Date,
                    transactionType: input.TransactionType,
                    paymentMethod: input.PaymentMethod,
                    status: input.Status,
                    destination: input.Destination),
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var installmentOutput = installmentServiceOutput is null ? null :
                new UpdateTransactionUseCaseOutputInstallment(
                    id: installmentServiceOutput.Id,
                    totalInstallments: installmentServiceOutput.TotalInstallments,
                    installments: installmentServiceOutput.Installments.Select(i => new UpdateTransactionUseCaseOutputInstallmentItem(
                        id: i.Id,
                        number: i.Number,
                        amount: i.Amount,
                        dueDate: i.DueDate,
                        status: i.Status,
                        paidAt: i.PaidAt,
                        updatedAt: i.UpdatedAt,
                        createdAt: i.CreatedAt))
                    .ToArray());

            var output = UpdateTransactionUseCaseOutput.Factory(
                id: transactionServiceOutput.Id,
                accountId: transactionServiceOutput.AccountId,
                categoryId: transactionServiceOutput.CategoryId,
                installment: installmentOutput,
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
