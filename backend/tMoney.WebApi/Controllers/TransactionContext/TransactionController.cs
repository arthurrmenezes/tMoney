using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
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
        [FromBody] CreateTransactionPayload input,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        if (!Guid.TryParse(input.CategoryId, out var category))
            throw new ArgumentException("Category ID inválido.");

        var response = await _transactionService.CreateTransactionServiceAsync(
            input: CreateTransactionServiceInput.Factory(
                accountId: IdValueObject.Factory(accountId),
                categoryId: IdValueObject.Factory(category),
                title: input.Title,
                description: input.Description,
                amount: input.Amount,
                date: input.Date,
                transactionType: input.TransactionType,
                paymentMethod: input.PaymentMethod,
                status: input.Status,
                destination: input.Destination),
            cancellationToken: cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Route("{transactionId}")]
    [Authorize]
    public async Task<IActionResult> GetTransactionByIdAsync(
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        var serviceResult = await _transactionService.GetTransactionByIdServiceAsync(
            transactionId: IdValueObject.Factory(transactionId),
            accountId: IdValueObject.Factory(accountId),
            cancellationToken: cancellationToken);

        return Ok(serviceResult);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTransactionsAsync(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var accountId = User.GetAccountId();

        var serviceResult = await _transactionService.GetAllTransactionsByAccountIdServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            pageNumber: pageNumber,
            pageSize: pageSize,
            cancellationToken: cancellationToken);

        return Ok(serviceResult);
    }
}
