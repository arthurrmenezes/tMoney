using tMoney.Infrastructure.Auth.Entities;

namespace tMoney.Infrastructure.Services.TokenService.Interfaces;

public interface ITokenService
{
    public string GenerateAcessToken(User user);
    public int GetTokenExpirationInSeconds();
}
