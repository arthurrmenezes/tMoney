using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Interfaces;
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
    [Route("register")]
    [AllowAnonymous]
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

        var response = await _authService.RegisterAccountServiceAsync(serviceInput, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Email))
            return BadRequest("O email não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(input.Password))
            return BadRequest("A senha não pode ser nula ou vazia.");

        var serviceInput = LoginServiceInput.Factory(
            email: input.Email,
            password: input.Password);

        var response = await _authService.LoginServiceAsync(serviceInput, cancellationToken);

        return Ok(response);
    }
}
