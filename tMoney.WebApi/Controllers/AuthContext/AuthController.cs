using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.AuthContext.Payloads;

namespace tMoney.WebApi.Controllers.AuthContext;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> RegisterAccountAsync([FromBody] RegisterAccountPayload input, CancellationToken cancellationToken)
    {
        if (input.Password != input.RePassword)
            return BadRequest("A senha e a confirmação não são iguais.");

        var serviceInput = RegisterAccountServiceInput.Factory(
                firstName: input.FirstName,
                lastName: input.LastName,
                email: input.Email,
                password: input.Password,
                rePassword: input.RePassword);

        var response = await _authService.RegisterAccountServiceAsync(
            input: serviceInput,
            cancellationToken: cancellationToken);

        return Ok(response);
    }
}
