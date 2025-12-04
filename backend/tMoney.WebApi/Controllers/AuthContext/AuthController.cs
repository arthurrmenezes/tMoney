using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        return Created("api/v1/accounts/profile", response);
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

    [HttpPost]
    [Route("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.RefreshToken))
            return BadRequest("O Refresh Token não pode ser nulo ou vazio.");

        var serviceInput = RefreshTokenServiceInput.Factory(
            refreshToken: input.RefreshToken);

        var response = await _authService.RefreshTokenServiceAsync(serviceInput, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _authService.LogoutServiceAsync(
            userId: userId,
            cancellationToken: cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Route("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string email, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("O email não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(token))
            return BadRequest("O token não pode ser nulo ou vazio.");

        await _authService.ConfirmEmailServiceAsync(email, token);

        return NoContent();
    }

    [HttpPost]
    [Route("resend-confirmation-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] ResendConfirmationEmailPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.Email))
            return BadRequest("O email não pode ser nulo ou vazio.");

        await _authService.ResendConfirmationEmailServiceAsync(input.Email, cancellationToken);

        return NoContent();
    }

    [HttpPost]
    [Route("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.CurrentPassword) || 
            string.IsNullOrEmpty(input.NewPassword) || 
            string.IsNullOrEmpty(input.ConfirmNewPassword))
            return BadRequest("Todos os campos são obrigatórios.");

        if (input.NewPassword != input.ConfirmNewPassword)
            throw new ArgumentException("A nova senha e a confirmação não são iguais.");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        await _authService.ChangePasswordServiceAsync(
            input: ChangePasswordServiceInput.Factory(
                userId: userId,
                currentPassword: input.CurrentPassword,
                newPassword: input.NewPassword),
            cancellationToken: cancellationToken);

        return NoContent();
    }

    [HttpPost]
    [Route("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.Email))
            return BadRequest("O email é obrigatório.");

        await _authService.ForgotPasswordServiceAsync(
            email: input.Email,
            cancellationToken: cancellationToken);

        return NoContent();
    }

    [HttpPost]
    [Route("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordAsync([FromQuery] string email, [FromQuery] string token, 
        [FromBody] ResetPasswordPayload input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("O email não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(token))
            return BadRequest("O token não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(input.NewPassword) || string.IsNullOrEmpty(input.ConfirmNewPassword))
            return BadRequest("Todos os campos são obrigatórios.");

        if (input.NewPassword != input.ConfirmNewPassword)
            return BadRequest("A nova senha e a confirmação não são iguais.");

        await _authService.ResetPasswordServiceAsync(
            input: ResetPasswordServiceInput.Factory(
                email: email,
                emailToken: token,
                newPassword: input.NewPassword),
            cancellationToken: cancellationToken);

        return NoContent();
    }
}
