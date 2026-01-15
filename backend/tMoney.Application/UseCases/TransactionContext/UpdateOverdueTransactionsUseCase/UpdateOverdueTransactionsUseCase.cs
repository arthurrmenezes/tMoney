using tMoney.Application.UseCases.Interfaces;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.UseCases.TransactionContext.UpdateOverdueTransactionsUseCase;

public sealed class UpdateOverdueTransactionsUseCase : IUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOverdueTransactionsUseCase(ITransactionRepository transactionRepository, IInstallmentRepository installmentRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _installmentRepository = installmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteUseCaseAsync(CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _installmentRepository.UpdateOverdueInstallmentsAsync(cancellationToken);

            await _transactionRepository.UpdateOverdueTransactionsAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
