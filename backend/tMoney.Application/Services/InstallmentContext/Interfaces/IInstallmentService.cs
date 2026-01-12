using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.InstallmentContext.Interfaces;

public interface IInstallmentService
{
    public Task<CreateInstallmentServiceOutput> CreateInstallmentServiceAsync(
        CreateInstallmentServiceInput input,
        CancellationToken cancellationToken);

    public Task<GetInstallmentServiceOutput> GetInstallmentByIdServiceAsync(
        IdValueObject installmentId,
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<GetAllInstallmentsByAccountIdServiceOutput[]> GetAllInstallmentsByTransactionIdServiceAsync(
        IdValueObject accountId,
        IdValueObject[] installmentIds,
        CancellationToken cancellationToken);

    public Task<UpdateInstallmentDetailsByIdServiceOutput> UpdateInstallmentDetailsByIdServiceAsync(
        IdValueObject installmentId,
        IdValueObject accountId,
        UpdateInstallmentDetailsByIdServiceInput input,
        CancellationToken cancellationToken);
}
