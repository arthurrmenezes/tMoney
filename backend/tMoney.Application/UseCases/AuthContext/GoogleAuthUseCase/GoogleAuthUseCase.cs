using Microsoft.AspNetCore.Identity;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Inputs;
using tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Outputs;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;
using tMoney.Infrastructure.Services.EmailService.Domain;
using tMoney.Infrastructure.Services.EmailService.Inputs;
using tMoney.Infrastructure.Services.EmailService.Interfaces;
using tMoney.Infrastructure.Services.OAuthService.Google.Interfaces;
using tMoney.Infrastructure.Services.TokenService.Interfaces;

namespace tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase;

public sealed class GoogleAuthUseCase : IUseCase<GoogleAuthUseCaseInput, GoogleAuthUseCaseOutput>
{
    private readonly IGoogleService _googleService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEmailService _emailService;

    public GoogleAuthUseCase(IGoogleService googleService, UserManager<User> userManager, IAuthService authService, IUnitOfWork unitOfWork,
        ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IAccountRepository accountRepository, 
        ICategoryRepository categoryRepository, IEmailService emailService)
    {
        _googleService = googleService;
        _userManager = userManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _emailService = emailService;
    }

    public async Task<GoogleAuthUseCaseOutput> ExecuteUseCaseAsync(GoogleAuthUseCaseInput input, CancellationToken cancellationToken)
    {
        var googleResult = await _googleService.ValidateGoogleTokenAsync(input.GoogleToken);

        var googleUser = await _userManager.FindByEmailAsync(googleResult.Email);
        if (googleUser is not null)
        {
            var accessToken = _tokenService.GenerateAccessToken(googleUser);
            var tokenExpirationInSeconds = _tokenService.GetAccessTokenExpiration();

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiration());

            var refreshTokenEntity = new RefreshToken(
                token: refreshToken,
                userId: googleUser.Id,
                expiresAt: refreshTokenExpiration);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var output = GoogleAuthUseCaseOutput.Factory(
                accountId: googleUser.AccountId.ToString(),
                firstName: null,
                lastName: null,
                email: googleUser.Email!,
                accessToken: accessToken,
                tokenType: "Bearer",
                expiresIn: tokenExpirationInSeconds,
                refreshToken: refreshToken,
                createdAt: null);

            return output;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var account = new Account(
                firstName: googleResult.FirstName,
                lastName: googleResult.LastName,
                email: googleResult.Email);

            await _accountRepository.AddAsync(account, cancellationToken);

            var user = new User
            {
                UserName = googleResult.Email,
                Email = googleResult.Email,
                AccountId = account.AccountId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Falha ao criar conta: {error}");
            }

            var defaultCategory = new Category(
                title: "Outros",
                type: CategoryType.Both,
                accountId: account.AccountId,
                isDefault: true);

            await _categoryRepository.AddAsync(defaultCategory, cancellationToken);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var tokenExpirationInSeconds = _tokenService.GetAccessTokenExpiration();

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiration());

            var refreshTokenEntity = new RefreshToken(
                token: refreshToken,
                userId: user.Id,
                expiresAt: refreshTokenExpiration);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var emailMessage = EmailTemplates.WelcomeEmailGoogleAuthTemplateMessageBody(account.FirstName);
            var emailSubject = EmailTemplates.WelcomeEmailGoogleAuthTemplateSubject();

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

            var output = GoogleAuthUseCaseOutput.Factory(
                accountId: account.AccountId.ToString(),
                firstName: account.FirstName,
                lastName: account.LastName,
                email: account.Email,
                accessToken: accessToken,
                tokenType: "Bearer",
                expiresIn: tokenExpirationInSeconds,
                refreshToken: refreshToken,
                createdAt: account.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
