using Microsoft.Extensions.DependencyInjection;

namespace tMoney.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplyApplicationDependencyInjection(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}
