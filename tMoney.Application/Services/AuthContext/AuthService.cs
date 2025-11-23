using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.Services.AuthContext.Outputs;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;
using tMoney.Infrastructure.Services.EmailService.Domain;
using tMoney.Infrastructure.Services.EmailService.Inputs;
using tMoney.Infrastructure.Services.EmailService.Interfaces;
using tMoney.Infrastructure.Services.TokenService.Interfaces;

namespace tMoney.Application.Services.AuthContext;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork, IAccountRepository accountRepository, 
        ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _emailService = emailService;
    }

    public async Task<RegisterAccountServiceOutput> RegisterAccountServiceAsync(RegisterAccountServiceInput input, CancellationToken cancellationToken)
    {
        var emailExists = await _userManager.FindByEmailAsync(input.Email);
        if (emailExists is not null)
            throw new ArgumentException("O email já está em uso.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var account = new Account(
                firstName: input.FirstName,
                lastName: input.LastName,
                email: input.Email);

            await _accountRepository.AddAsync(account, cancellationToken);

            var user = new User
            {
                UserName = input.Email,
                Email = input.Email,
                AccountId = account.AccountId,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Falha ao criar conta: {error}");
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailConfirmationEncodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));
            var emailConfirmationLink = $"https://localhost:7159/api/v1/auth/confirm-email?email={user.Email}&token={emailConfirmationEncodedToken}";

            var emailMessage = EmailTemplates.WelcomeEmailTemplateMessageBody(account.FirstName, emailConfirmationLink);
            var emailSubject = EmailTemplates.WelcomeEmailTemplateSubject();

            var emailInput = SendEmailServiceInput.Factory(
                to: [
                    new SendEmailServiceInputTo(
                        name: account.FirstName,
                        email: account.Email),
                    ],
                htmlContent: emailMessage,
                subject: emailSubject);

            await _emailService.SendEmailAsync(
                input: emailInput,
                cancellationToken: cancellationToken);

            var output = RegisterAccountServiceOutput.Factory(
                accountId: account.AccountId,
                firstName: account.FirstName,
                lastName: account.LastName,
                email: account.Email,
                balance: account.Balance,
                createdAt: account.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<LoginServiceOutput> LoginServiceAsync(LoginServiceInput input, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user is null)
            throw new UnauthorizedAccessException("Email ou senha incorreta.");

        var verifyCredentials = await _signInManager.CheckPasswordSignInAsync(user, input.Password, true);

        if (verifyCredentials.IsLockedOut)
            throw new InvalidOperationException("Muitas tentativas falhas. Tente novamente mais tarde.");
        
        if (!verifyCredentials.Succeeded)
            throw new UnauthorizedAccessException("Email ou senha incorreta.");

        var acessToken = _tokenService.GenerateAccessToken(user);
        var tokenExpirationInSeconds = _tokenService.GetAccessTokenExpiration();

        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiration());

        var refreshTokenEntity = new RefreshToken(
            token: refreshToken,
            userId: user.Id,
            expiresAt: refreshTokenExpiration);

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = LoginServiceOutput.Factory(
            accessToken: acessToken,
            tokenType: "Bearer",
            expiresIn: tokenExpirationInSeconds,
            refreshToken: refreshToken);

        return output;
    }

    public async Task<RefreshTokenServiceOutput> RefreshTokenServiceAsync(RefreshTokenServiceInput input, CancellationToken cancellationToken)
    {
        var currentRefreshToken = await _refreshTokenRepository.GetByTokenAsync(input.RefreshToken, cancellationToken);
        if (currentRefreshToken is null)
            throw new KeyNotFoundException("Refresh token não encontrado.");

        if (!currentRefreshToken.IsActive())
            throw new InvalidOperationException("Refresh token inválido, expirado ou revogado.");

        var user = await _userManager.FindByIdAsync(currentRefreshToken.UserId);
        if (user is null)
            throw new InvalidOperationException("Usuário associado ao refresh token não encontrado.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken(
                token: newRefreshToken,
                userId: user.Id,
                expiresAt: DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiration()));

            currentRefreshToken.Revoke(newRefreshToken);

            _refreshTokenRepository.Update(currentRefreshToken);

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = RefreshTokenServiceOutput.Factory(
                accessToken: newAccessToken,
                refreshToken: newRefreshToken);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task LogoutServiceAsync(string userId, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllByUserIdAsync(userId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ConfirmEmailServiceAsync(string email, string emailToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new KeyNotFoundException("Email inválido.");

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(emailToken));

        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded)
            throw new InvalidOperationException($"Não foi possível confirmar o email. O token pode ter expirado ou estar inválido.");
    }
}
