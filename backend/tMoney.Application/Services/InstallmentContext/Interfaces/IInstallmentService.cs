using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.InstallmentContext.Interfaces;

public interface IInstallmentService
{
    public Task<CreateInstallmentServiceOutput> CreateInstallmentServiceAsync(
        CreateInstallmentServiceInput input,
        CancellationToken cancellationToken);

    public Task<GetInstallmentServiceOutput> GetInstallmentServiceAsync(
        IdValueObject installmentId,
        IdValueObject accountId,
        CancellationToken cancellationToken);
}
