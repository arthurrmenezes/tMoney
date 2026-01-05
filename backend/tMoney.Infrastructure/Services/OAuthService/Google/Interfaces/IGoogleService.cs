using tMoney.Infrastructure.Services.OAuthService.Google.Outputs;

namespace tMoney.Infrastructure.Services.OAuthService.Google.Interfaces;

public interface IGoogleService
{
    public Task<ValidateGoogleTokenOutput> ValidateGoogleTokenAsync(string token);
}
