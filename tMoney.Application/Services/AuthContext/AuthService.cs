using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.Services.AuthContext.Outputs;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;
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

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork, 
        IAccountRepository accountRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
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
                AccountId = account.AccountId
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Falha ao criar conta: {error}");
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

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
            throw new ArgumentException("Email ou senha incorreta.");

        var verifyCredentials = await _signInManager.CheckPasswordSignInAsync(user, input.Password, true);

        if (verifyCredentials.IsLockedOut)
            throw new ArgumentException("Muitas tentativas falhas. Tente novamente mais tarde.");

        if (!verifyCredentials.Succeeded)
            throw new ArgumentException("Email ou senha incorreta.");

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
}
