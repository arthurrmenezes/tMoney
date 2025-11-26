using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Mappings;

namespace tMoney.Infrastructure.Data;

public sealed class DataContext : IdentityDbContext<User>
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountMapping());
        builder.ApplyConfiguration(new UserMapping());
        builder.ApplyConfiguration(new RefreshTokenMapping());
        base.OnModelCreating(builder);
    }
}
