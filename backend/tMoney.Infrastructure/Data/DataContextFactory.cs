using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace tMoney.Infrastructure.Data;

public sealed class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? throw new DirectoryNotFoundException("Erro ao obter variável de ambiente.");

        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../tMoney.WebApi");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile($"appsettings.{environment}.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration["Database:ConnectionString"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string não encontrada.");

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsql => npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                .MigrationsAssembly("tMoney.Infrastructure"));

        return new DataContext(optionsBuilder.Options);
    }
}
