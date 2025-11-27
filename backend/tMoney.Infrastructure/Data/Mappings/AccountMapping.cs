using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.AccountId);

        #region Properties Configuration

        builder.Property(a => a.AccountId)
            .IsRequired()
            .HasConversion(
                a => a.Id, 
                a => IdValueObject.Factory(a));

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasColumnName("first_name")
            .HasMaxLength(FirstNameValueObject.MaxLength)
            .HasConversion(
                a => a.FirstName,
                a => FirstNameValueObject.Factory(a))
            .ValueGeneratedNever();

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasColumnName("last_name")
            .HasMaxLength(LastNameValueObject.MaxLength)
            .HasConversion(
                a => a.LastName,
                a => LastNameValueObject.Factory(a))
            .ValueGeneratedNever();

        builder.Property(a => a.Email)
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(EmailValueObject.MaxLength)
            .HasConversion(
                a => a.Email,
                a => EmailValueObject.Factory(a))
            .ValueGeneratedNever();

        builder.Property(a => a.Balance)
            .IsRequired()
            .HasColumnName("balance")
            .HasPrecision(18, 2)
            .ValueGeneratedNever();

        builder.Property(a => a.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at")
            .ValueGeneratedNever();

        builder.Property(a => a.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        #endregion
    }
}
