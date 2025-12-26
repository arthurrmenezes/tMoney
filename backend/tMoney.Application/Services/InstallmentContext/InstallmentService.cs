using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Application.Services.InstallmentContext;

public class InstallmentService : IInstallmentService
{
    private readonly IInstallmentRepository _installmentRepository;

    public InstallmentService(IInstallmentRepository installmentRepository)
    {
        _installmentRepository = installmentRepository;
    }

    public async Task<CreateInstallmentServiceOutput> CreateInstallmentServiceAsync(CreateInstallmentServiceInput input, CancellationToken cancellationToken)
    {
        var installment = new Installment(
            accountId: input.AccountId,
            totalInstallments: input.TotalInstallments,
            totalAmount: input.TotalAmount,
            firstPaymentDate: input.FirstPaymentDate,
            status: input.Status);
        
        installment.GenerateInstallments();

        if (installment.Status == PaymentStatus.Paid)
            installment.PayAllInstallments();

        await _installmentRepository.AddAsync(installment, cancellationToken);

        var installmentItemOutput = installment.Installments.Select(i => new CreateInstallmentServiceOutputInstallmentItem(
            id: i.Id,
            number: i.Number,
            amount: i.Amount,
            dueDate: i.DueDate,
            status: i.Status.ToString(),
            paidAt: i.PaidAt,
            updatedAt: i.UpdatedAt,
            createdAt: i.CreatedAt)).ToArray();

        var output = CreateInstallmentServiceOutput.Factory(
            id: installment.Id,
            accountId: installment.AccountId,
            totalInstallments: installment.TotalInstallments,
            totalAmount: installment.TotalAmount,
            firstPaymentDate: installment.FirstPaymentDate,
            status: installment.Status.ToString(),
            installmentItems: installmentItemOutput,
            updatedAt: installment.UpdatedAt,
            createdAt: installment.CreatedAt);

        return output;
    }
}
