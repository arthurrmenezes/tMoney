using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Outputs;

namespace tMoney.Application.Services.InstallmentContext.Interfaces;

public interface IInstallmentService
{
    public Task<CreateInstallmentServiceOutput> CreateInstallmentServiceAsync(
        CreateInstallmentServiceInput input,
        CancellationToken cancellationToken);
}
