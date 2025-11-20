using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Auth.Entities;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.AccountId).IsUnique();

        #region Properties Configuration

        builder.Property(u => u.AccountId)
            .IsRequired()
            .HasColumnName("account_id")
            .HasConversion(
                a => a.Id,
                a => IdValueObject.Factory(a))
            .ValueGeneratedNever();

        #endregion
    }
}
