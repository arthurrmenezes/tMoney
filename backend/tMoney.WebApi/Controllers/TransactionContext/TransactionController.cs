using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Outputs;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.TransactionContext.Payloads;
using tMoney.WebApi.Extensions;

namespace tMoney.WebApi.Controllers.TransactionContext;

[ApiController]
[Route("api/v1/transactions")]
public class TransactionController : ControllerBase
{
    ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTransactionAsync(
        [FromServices] IUseCase<CreateTransactionUseCaseInput, CreateTransactionUseCaseOutput> useCase,
        [FromBody] CreateTransactionPayload input,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        if (!Guid.TryParse(input.CategoryId, out var categoryId))
            throw new ArgumentException("Category ID inválido.");

        var hasInstallment = input.HasInstallment is null ? null :
            new CreateTransactionUseCaseInputInstallment(
                totalInstallments: input.HasInstallment.TotalInstallments);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: CreateTransactionUseCaseInput.Factory(
                accountId: IdValueObject.Factory(accountId),
                categoryId: IdValueObject.Factory(categoryId),
                title: input.Title,
                description: input.Description,
                amount: input.Amount,
                date: input.Date,
                transactionType: input.TransactionType,
                paymentMethod: input.PaymentMethod,
                status: input.Status,
                destination: input.Destination,
                hasInstallment: hasInstallment),
            cancellationToken: cancellationToken);

        return CreatedAtAction("GetTransactionById", new { transactionId = useCaseResult.Id }, useCaseResult);
    }

    [HttpGet]
    [Route("{transactionId}")]
    [Authorize]
    public async Task<IActionResult> GetTransactionByIdAsync(
        [FromServices] IUseCase<GetTransactionUseCaseInput, GetTransactionUseCaseOutput> useCase,
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: GetTransactionUseCaseInput.Factory(
                transactionId: IdValueObject.Factory(transactionId),
                accountId: IdValueObject.Factory(accountId)),
            cancellationToken: cancellationToken);

        return Ok(useCaseResult);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTransactionsAsync(
        [FromServices] IUseCase<GetAllTransactionsUseCaseInput, GetAllTransactionsUseCaseOutput> useCase,
        CancellationToken cancellationToken,
        [FromQuery] GetAllTransactionsPayload input,
        [FromQuery] int? pageNumber = 1,
        [FromQuery] int? pageSize = 10)
    {
        if (pageNumber < 1 || pageNumber is null) pageNumber = 1;
        if (pageSize < 1 || pageSize is null) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var accountId = User.GetAccountId();

        var category = input.CategoryId is not null ? IdValueObject.Factory(input.CategoryId.Id) : null;

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: GetAllTransactionsUseCaseInput.Factory(
                accountId: IdValueObject.Factory(accountId),
                pageNumber: pageNumber is not null ? pageNumber.Value : 0,
                pageSize: pageSize is not null ? pageSize.Value : 0,
                transactionType: input.TransactionType,
                categoryId: category,
                paymentMethod: input.PaymentMethod,
                paymentStatus: input.PaymentStatus,
                startDate: input.StartDate,
                endDate: input.EndDate,
                minValue: input.MinValue,
                maxValue: input.MaxValue,
                textSearch: input.TextSearch,
                hasInstallment: input.HasInstallment),
            cancellationToken: cancellationToken);

        return Ok(useCaseResult);
    }

    [HttpPatch]
    [Route("{transactionId}")]
    [Authorize]
    public async Task<IActionResult> UpdateTransactionDetailsAsync(
        [FromServices] IUseCase<UpdateTransactionUseCaseInput, UpdateTransactionUseCaseOutput> useCase,
        [FromRoute] Guid transactionId,
        [FromBody] UpdateTransactionDetailsPayload input,
        CancellationToken cancellationToken)
    {
        if (input.CategoryId is null &&
            input.Title is null &&
            input.Description is null &&
            input.Amount is null &&
            input.Date is null &&
            input.TransactionType is null &&
            input.PaymentMethod is null &&
            input.Status is null &&
            input.Destination is null &&
            input.Installment is null)
            throw new ArgumentException("Informe pelo menos um campo para atualizar.");

        var accountId = User.GetAccountId();

        Guid? categoryId = null;
        if (input.CategoryId is not null)
        {
            if (!Guid.TryParse(input.CategoryId, out var parsedCategory))
                throw new ArgumentException("Category ID inválido.");

            categoryId = parsedCategory;
        }

        UpdateTransactionUseCaseInputInstallment? installmentUseCaseInput = null;
        if (input.Installment is not null)
            installmentUseCaseInput = new UpdateTransactionUseCaseInputInstallment(
                totalInstallments: input.Installment.TotalInstallments);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: UpdateTransactionUseCaseInput.Factory(
                transactionId: IdValueObject.Factory(transactionId),
                accountId: IdValueObject.Factory(accountId),
                categoryId: categoryId is null ? null : IdValueObject.Factory(categoryId.Value),
                title: input.Title,
                description: input.Description,
                amount: input.Amount,
                date: input.Date,
                transactionType: input.TransactionType,
                paymentMethod: input.PaymentMethod,
                status: input.Status,
                destination: input.Destination,
                installment: installmentUseCaseInput),
            cancellationToken: cancellationToken);

        return Ok(useCaseResult);
    }

    [HttpDelete]
    [Route("{transactionId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTransactionByIdAsync(
        [FromServices] IUseCase<DeleteTransactionUseCaseInput, bool> useCase,
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        await useCase.ExecuteUseCaseAsync(
            input: DeleteTransactionUseCaseInput.Factory(
                transactionId: IdValueObject.Factory(transactionId),
                accountId: IdValueObject.Factory(accountId)),
            cancellationToken: cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Route("financial-summary")]
    [Authorize]
    public async Task<IActionResult> GetFinancialSummaryAsync(
        CancellationToken cancellationToken,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var accountId = User.GetAccountId();

        var serviceResult = await _transactionService.GetFinancialSummaryServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            startDate: startDate,
            endDate: endDate,
            cancellationToken: cancellationToken);

        return Ok(serviceResult);
    }
}
