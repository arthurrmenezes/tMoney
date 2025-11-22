using System.Security.Claims;
using tMoney.Infrastructure.Auth.Entities;

namespace tMoney.Infrastructure.Services.TokenService.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    public int GetAccessTokenExpiration();
    public int GetRefreshTokenExpiration();
}
