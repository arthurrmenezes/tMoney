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
}
