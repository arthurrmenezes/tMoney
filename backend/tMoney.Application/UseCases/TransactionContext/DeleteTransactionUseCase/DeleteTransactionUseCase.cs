using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase.Inputs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase;

public sealed class DeleteTransactionUseCase : IUseCase<DeleteTransactionUseCaseInput, bool>
{
    private readonly ITransactionService _transactionService;
    private readonly IInstallmentService _installmentService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionUseCase(ITransactionService transactionService, IInstallmentService installmentService, IUnitOfWork unitOfWork)
    {
        _transactionService = transactionService;
        _installmentService = installmentService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteUseCaseAsync(DeleteTransactionUseCaseInput input, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var transactionServiceOutput = await _transactionService.DeleteTransactionByIdServiceAsync(
                transactionId: input.TransactionId,
                accountId: input.AccountId,
                cancellationToken: cancellationToken);

            if (transactionServiceOutput.Installment is not null)
            {
                if (!Guid.TryParse(transactionServiceOutput.Installment, out var installmentId))
                    throw new ArgumentException("Installment ID inválido.");

                await _installmentService.DeleteInstallmentByIdServiceAsync(
                    installmentId: IdValueObject.Factory(installmentId),
                    accountId: input.AccountId,
                    cancellationToken: cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
