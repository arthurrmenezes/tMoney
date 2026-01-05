using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using tMoney.Infrastructure.Services.OAuthService.Google.Interfaces;
using tMoney.Infrastructure.Services.OAuthService.Google.Outputs;

namespace tMoney.Infrastructure.Services.OAuthService.Google;

public class GoogleService : IGoogleService
{
    private readonly IConfiguration _configuration;

    public GoogleService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ValidateGoogleTokenOutput> ValidateGoogleTokenAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[]
            {
                _configuration["OAuth:Google:ClientId"]
            }
        };

        var result = await GoogleJsonWebSignature.ValidateAsync(token, settings);

        var output = ValidateGoogleTokenOutput.Factory(
            firstName: result.Name,
            lastName: result.FamilyName,
            email: result.Email);

        return output;
    }
}
