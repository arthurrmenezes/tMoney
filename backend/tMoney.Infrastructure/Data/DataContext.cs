using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Mappings;

namespace tMoney.Infrastructure.Data;

public sealed class DataContext : IdentityDbContext<User>
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<InstallmentItem> InstallmentItems { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountMapping());
        builder.ApplyConfiguration(new UserMapping());
        builder.ApplyConfiguration(new RefreshTokenMapping());
        builder.ApplyConfiguration(new CategoryMapping());
        builder.ApplyConfiguration(new TransactionMapping());
        builder.ApplyConfiguration(new InstallmentMapping());
        builder.ApplyConfiguration(new InstallmentItemMapping());
        base.OnModelCreating(builder);
    }
}
