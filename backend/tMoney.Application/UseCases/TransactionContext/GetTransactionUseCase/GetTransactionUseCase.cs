using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase;

public sealed class GetTransactionUseCase : IUseCase<GetTransactionUseCaseInput, GetTransactionUseCaseOutput>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;

    public GetTransactionUseCase(ITransactionService transactionService, IInstallmentService installmentService)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
    }

    public async Task<GetTransactionUseCaseOutput> ExecuteUseCaseAsync(GetTransactionUseCaseInput input, CancellationToken cancellationToken)
    {
        var transactionServiceOutput = await _transactionService.GetTransactionByIdServiceAsync(
            transactionId: input.TransactionId,
            accountId: input.AccountId,
            cancellationToken: cancellationToken);

        GetInstallmentServiceOutput? installmentServiceOutput = null;

        if (transactionServiceOutput.InstallmentId is not null)
        {
            if (!Guid.TryParse(transactionServiceOutput.InstallmentId, out var installmentId))
                throw new ArgumentException("Installment ID inválido.");

            installmentServiceOutput = await _installmentService.GetInstallmentByIdServiceAsync(
                installmentId: IdValueObject.Factory(installmentId),
                accountId: input.AccountId,
                cancellationToken: cancellationToken);
        }

        var installmentItemsOutput = installmentServiceOutput?.Installments.Select(i => new GetTransactionUseCaseOutputInstallmentItem(
            id: i.Id,
            number: i.Number,
            amount: i.Amount,
            dueDate: i.DueDate,
            status: i.Status,
            paidAt: i.PaidAt,
            updatedAt: i.UpdatedAt,
            createdAt: i.CreatedAt))
            .ToArray();

        var installmentOutput = installmentServiceOutput is null ? null :
            new GetTransactionUseCaseOutputInstallment(
                id: installmentServiceOutput.Id,
                totalInstallments: installmentServiceOutput.TotalInstallments,
                totalAmount: installmentServiceOutput.TotalAmount,
                firstPaymentDate: installmentServiceOutput.FirstPaymentDate,
                status: installmentServiceOutput.Status,
                installments: installmentItemsOutput!);

        var output = GetTransactionUseCaseOutput.Factory(
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
            installment: installmentOutput,
            updatedAt: transactionServiceOutput.UpdatedAt,
            createdAt: transactionServiceOutput.CreatedAt);

        return output;
    }
}
