using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Domain.ValueObjects;

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
            return Unauthorized("Token inválido ou sem identificador de conta.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            return BadRequest("ID da conta inválido no token");

        var accountIdValueObject = IdValueObject.Factory(accountIdGuid);

        var response = await _accountService.GetAccountDetailsServiceAsync(
            accountId: accountIdValueObject,
            cancellationToken: cancellationToken);

        return Ok(response);
    }
}
