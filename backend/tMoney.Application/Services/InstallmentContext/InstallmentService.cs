using tMoney.Application.Services.InstallmentContext.Inputs;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.InstallmentContext.Outputs;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Application.Services.InstallmentContext;

public class InstallmentService : IInstallmentService
{
    private readonly IInstallmentRepository _installmentRepository;

    public InstallmentService(IInstallmentRepository installmentRepository)
    {
        _installmentRepository = installmentRepository;
    }

    public async Task<CreateInstallmentServiceOutput> CreateInstallmentServiceAsync(
        CreateInstallmentServiceInput input, 
        CancellationToken cancellationToken)
    {
        var installment = new Installment(
            accountId: input.AccountId,
            totalInstallments: input.TotalInstallments,
            totalAmount: input.TotalAmount,
            firstPaymentDate: input.FirstPaymentDate,
            status: input.Status);

        //installment.GenerateInstallments(input.InvoiceIds, input.Status);

        await _installmentRepository.AddAsync(installment, cancellationToken);

        var installmentItemOutput = installment.Installments.Select(i => CreateInstallmentServiceOutputInstallmentItem.Factory(
            id: i.Id.ToString(),
            invoiceId: i.InvoiceId is null ? null : i.InvoiceId.ToString(),
            number: i.Number,
            amount: i.Amount,
            dueDate: i.DueDate,
            status: i.Status.ToString(),
            paidAt: i.PaidAt,
            updatedAt: i.UpdatedAt,
            createdAt: i.CreatedAt)).ToArray();

        var output = CreateInstallmentServiceOutput.Factory(
            id: installment.Id.ToString(),
            accountId: installment.AccountId.ToString(),
            totalInstallments: installment.TotalInstallments,
            totalAmount: installment.TotalAmount,
            firstPaymentDate: installment.FirstPaymentDate,
            status: installment.Status.ToString(),
            installmentItems: installmentItemOutput,
            updatedAt: installment.UpdatedAt,
            createdAt: installment.CreatedAt);

        return output;
    }

    public async Task<GetInstallmentServiceOutput> GetInstallmentByIdServiceAsync(
        IdValueObject installmentId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var installment = await _installmentRepository.GetByIdAsync(installmentId.Id, accountId.Id, false, cancellationToken);
        if (installment is null)
            throw new KeyNotFoundException("Parcelamento não encontrado");

        var installmentItemsOutput = installment.Installments
            .OrderBy(i => i.Number)
            .Select(i => GetInstallmentServiceOutputInstallmentItem.Factory(
                id: i.Id.ToString(),
                installmentId: i.InstallmentId.ToString(),
                invoiceId: i.InvoiceId?.ToString(),
                number: i.Number,
                amount: i.Amount,
                dueDate: i.DueDate,
                status: i.Status.ToString(),
                paidAt: i.PaidAt,
                updatedAt: i.UpdatedAt,
                createdAt: i.CreatedAt))
            .ToArray();

        var output = GetInstallmentServiceOutput.Factory(
            id: installment.Id.ToString(),
            accountId: installment.AccountId.ToString(),
            totalInstallments: installment.TotalInstallments,
            totalAmount: installment.TotalAmount,
            firstPaymentDate: installment.FirstPaymentDate,
            status: installment.Status.ToString(),
            installments: installmentItemsOutput,
            updatedAt: installment.UpdatedAt,
            createdAt: installment.CreatedAt);

        return output;
    }

    public async Task<GetAllInstallmentsByAccountIdServiceOutput[]> GetAllInstallmentsByTransactionIdServiceAsync(
        IdValueObject accountId,
        IdValueObject[] installmentIds, 
        CancellationToken cancellationToken)
    {
        Installment[]? installments = null;
        if (installmentIds.Any())
            installments = await _installmentRepository.GetAllByInstallmentIdAsync(accountId.Id, installmentIds.Select(i => i.Id), cancellationToken);

        var output = installments is null ? Array.Empty<GetAllInstallmentsByAccountIdServiceOutput>() : installments.Select(i => GetAllInstallmentsByAccountIdServiceOutput.Factory(
            id: i.Id.ToString(),
            accountId: i.AccountId.ToString(),
            totalInstallments: i.TotalInstallments,
            totalAmount: i.TotalAmount,
            firstPaymentDate: i.FirstPaymentDate,
            status: i.Status.ToString(),
            installments: i.Installments
            .OrderBy(ii => ii.Number)
            .Select(ii => GetAllInstallmentsByAccountIdServiceOutputInstallmentItem.Factory(
                id: ii.Id.ToString(),
                invoiceId: ii.InvoiceId?.ToString(),
                number: ii.Number,
                amount: ii.Amount,
                dueDate: ii.DueDate,
                status: ii.Status.ToString(),
                paidAt: ii.PaidAt,
                updatedAt: ii.UpdatedAt,
                createdAt: ii.CreatedAt)).ToArray(),
            updatedAt: i.UpdatedAt,
            createdAt: i.CreatedAt))
            .ToArray();

        return output;
    }

    public async Task<UpdateInstallmentDetailsByIdServiceOutput> UpdateInstallmentDetailsByIdServiceAsync(
        IdValueObject installmentId, 
        IdValueObject accountId, 
        UpdateInstallmentDetailsByIdServiceInput input, 
        CancellationToken cancellationToken)
    {
        var installment = await _installmentRepository.GetByIdAsync(installmentId.Id, accountId.Id, true, cancellationToken);
        if (installment is null)
            throw new KeyNotFoundException("Parcelamento não encontrado");

        installment.UpdateInstallmentDetails(input.TotalInstallments, input.TotalAmount, input.FirstPaymentDate, input.Status);

        _installmentRepository.Update(installment);

        var output = UpdateInstallmentDetailsByIdServiceOutput.Factory(
            id: installment.Id.ToString(),
            accountId: installment.AccountId.ToString(),
            totalInstallments: installment.TotalInstallments,
            totalAmount: installment.TotalAmount,
            firstPaymentDate: installment.FirstPaymentDate,
            status: installment.Status.ToString(),
            installments: installment.Installments.Select(i => new UpdateInstallmentDetailsByIdServiceOutputInstallment(
                id: i.Id.ToString(),
                number: i.Number,
                amount: i.Amount,
                dueDate: i.DueDate,
                status: i.Status.ToString(),
                paidAt: i.PaidAt,
                updatedAt: i.UpdatedAt,
                createdAt: i.CreatedAt))
            .ToArray(),
            updatedAt: installment.UpdatedAt!.Value,
            createdAt: installment.CreatedAt);

        return output;
    }

    public async Task DeleteInstallmentByIdServiceAsync(
        IdValueObject installmentId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var installment = await _installmentRepository.GetByIdAsync(installmentId.Id, accountId.Id, true, cancellationToken);
        if (installment is null)
            throw new KeyNotFoundException("Parcelamento não encontrado");

        _installmentRepository.Delete(installment);
    }
}
