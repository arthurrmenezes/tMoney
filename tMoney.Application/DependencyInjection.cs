using Microsoft.Extensions.DependencyInjection;
using tMoney.Application.Services.AuthContext;
using tMoney.Application.Services.AuthContext.Interfaces;

namespace tMoney.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplyApplicationDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthService, AuthService>();

        return serviceCollection;
    }
}
