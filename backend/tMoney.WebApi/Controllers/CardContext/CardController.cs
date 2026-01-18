using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.CardContext.Inputs;
using tMoney.Application.Services.CardContext.Interfaces;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.CardContext.Payloads;
using tMoney.WebApi.Extensions;

namespace tMoney.WebApi.Controllers.CardContext;

[ApiController]
[Route("api/v1/cards")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCardAsync(
        [FromBody] CreateCardPayload input,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        var creditCard = input.CreditCard is null ? null :
            CreateCardServiceInputCreditCard.Factory(
                limit: input.CreditCard.Limit,
                closeDay: input.CreditCard.CloseDay,
                dueDay: input.CreditCard.DueDay);

        var serviceResult = await _cardService.CreateCardServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            input: CreateCardServiceInput.Factory(
                name: input.Name,
                creditCard: creditCard),
            cancellationToken: cancellationToken);

        return CreatedAtAction("GetCardById", new { cardId = serviceResult.Id }, serviceResult);
    }

    [HttpGet]
    [Route("{cardId}")]
    [Authorize]
    public async Task<IActionResult> GetCardByIdAsync(
        [FromRoute] Guid cardId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        var serviceResult = await _cardService.GetCardByIdServiceAsync(
            cardId: IdValueObject.Factory(cardId),
            accountId: IdValueObject.Factory(accountId),
            cancellationToken: cancellationToken);

        return Ok(serviceResult);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCardsByAccountIdAsync(
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (pageNumber < 1 || pageNumber is null) pageNumber = 1;
        if (pageSize < 1 || pageSize is null) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var accountId = User.GetAccountId();

        var serviceResult = await _cardService.GetAllCardsByAccountIdServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            pageNumber: pageNumber.Value,
            pageSize: pageSize.Value,
            cancellationToken: cancellationToken);

        return Ok(serviceResult);
    }
}
