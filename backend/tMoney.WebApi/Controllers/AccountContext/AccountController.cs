using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.AccountContext.Inputs;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.AccountContext.Payloads;

namespace tMoney.WebApi.Controllers.AccountContext;

[ApiController]
[Route("api/v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Route("profile")]
    [Authorize]
    public async Task<IActionResult> GetAccountDetailsAsync(CancellationToken cancellationToken)
    {
        var accountIdString = User.FindFirst("accountId")?.Value;
        if (string.IsNullOrEmpty(accountIdString))
            throw new UnauthorizedAccessException("Token inválido ou sem identificador de conta.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            throw new ArgumentException("ID da conta inválido no token");

        var accountIdValueObject = IdValueObject.Factory(accountIdGuid);

        var response = await _accountService.GetAccountDetailsServiceAsync(
            accountId: accountIdValueObject,
            cancellationToken: cancellationToken);

        return Ok(response);
    }

    [HttpPatch]
    [Route("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateAccountDetailsAsync([FromBody] UpdateAccountDetailsPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.FirstName) && string.IsNullOrWhiteSpace(input.LastName))
            throw new ArgumentException("É preciso atualizar pelo menos um campo.");

        var accountIdString = User.FindFirst("accountId")?.Value;
        if (string.IsNullOrEmpty(accountIdString))
            throw new UnauthorizedAccessException("Token inválido ou sem identificador de conta.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            throw new ArgumentException("ID da conta inválido no token");

        var accountIdValueObject = IdValueObject.Factory(accountIdGuid);

        var response = await _accountService.UpdateAccountDetailsServiceAsync(
            accountId: accountIdValueObject,
            input: UpdateAccountDetailsServiceInput.Factory(
                firstName: input.FirstName,
                lastName: input.LastName),
            cancellationToken: cancellationToken);

        return Ok(response);
    }
}
