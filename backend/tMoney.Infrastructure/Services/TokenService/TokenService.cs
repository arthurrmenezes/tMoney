using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Services.TokenService.Interfaces;

namespace tMoney.Infrastructure.Services.TokenService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public string GenerateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:PrivateKey"]! ??
            throw new InvalidOperationException("Erro: a 'private key' não foi configurada."));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("accountId", user.AccountId.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(GetAccessTokenExpiration()),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public int GetAccessTokenExpiration() => _configuration.GetValue<int>("JwtSettings:AccessTokenExpirationInSeconds");

    public int GetRefreshTokenExpiration() => _configuration.GetValue<int>("JwtSettings:RefreshTokenExpirationInDays");

    public async Task DeleteInvalidRefreshTokensAsync(CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.DeleteInvalidRefreshTokensAsync(cancellationToken);
    }
}
