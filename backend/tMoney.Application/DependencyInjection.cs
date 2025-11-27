using Microsoft.Extensions.DependencyInjection;
using tMoney.Application.Services.AccountContext;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Application.Services.AuthContext;
using tMoney.Application.Services.AuthContext.Interfaces;

namespace tMoney.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplyApplicationDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IAccountService, AccountService>();

        return serviceCollection;
    }
}
