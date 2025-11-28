using Microsoft.Extensions.DependencyInjection;
using tMoney.Application.Services.AccountContext;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Application.Services.AuthContext;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.Services.CategoryContext;
using tMoney.Application.Services.CategoryContext.Interface;

namespace tMoney.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplyApplicationDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<ICategoryService, CategoryService>();

        return serviceCollection;
    }
}
