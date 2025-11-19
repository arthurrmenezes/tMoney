using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data;
using tMoney.Infrastructure.Data.UnitOfWork;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ApplyInfrastructureDependencyInjection(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<DataContext>(optionsAction => optionsAction.UseNpgsql(
            connectionString: connectionString,
            npgsqlOptionsAction: options => options.MigrationsAssembly("tMoney.Infrastructure")));

        serviceCollection
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<DataContext>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }
}
