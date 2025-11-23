using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data;
using tMoney.Infrastructure.Data.Repositories;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;
using tMoney.Infrastructure.Services.EmailService;
using tMoney.Infrastructure.Services.EmailService.Interfaces;
using tMoney.Infrastructure.Services.TokenService;
using tMoney.Infrastructure.Services.TokenService.Interfaces;

namespace tMoney.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ApplyInfrastructureDependencyInjection(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        #region DbContext and Database Configuration

        var connectionString = configuration["Database:ConnectionString"];

        serviceCollection.AddDbContext<DataContext>(optionsAction => optionsAction.UseNpgsql(
            connectionString: connectionString,
            npgsqlOptionsAction: options => options.MigrationsAssembly("tMoney.Infrastructure")));

        #endregion

        #region Identity Configuration

        serviceCollection
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSettings:PrivateKey")!);
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
                };
            });

        #endregion

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        serviceCollection.AddScoped<ITokenService, TokenService>();

        serviceCollection.AddHttpClient<IEmailService, EmailService>();

        #region Repositories Configuration

        serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
        serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        #endregion

        return serviceCollection;
    }
}
