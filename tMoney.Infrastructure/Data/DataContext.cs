using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tMoney.Infrastructure.Auth.Entities;

namespace tMoney.Infrastructure.Data;

public sealed class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
}
