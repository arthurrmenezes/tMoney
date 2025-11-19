using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data;

namespace tMoney.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ApplyInfrastructureDependencyInjection(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<DataContext>(optionsAction => optionsAction.UseNpgsql(
            connectionString: connectionString,
            npgsqlOptionsAction: options => options.MigrationsAssembly("tMoney.Infrastructure")));

        serviceCollection
            .AddIdentityCore<User>(optinos =>
            {
                optinos.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<DataContext>();

        return serviceCollection;
    }
}
