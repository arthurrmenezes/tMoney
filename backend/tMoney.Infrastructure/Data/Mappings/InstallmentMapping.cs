using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class InstallmentMapping : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("installments");

        builder.HasKey(i => i.Id);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(i => i.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Installments)
            .WithOne()
            .HasForeignKey(i => i.InstallmentId)
            .OnDelete(DeleteBehavior.Cascade);

        var idConverter = new ValueConverter<IdValueObject, Guid>(
            i => i.Id,
            i => IdValueObject.Factory(i));

        #region Properties Configuration

        builder.Property(i => i.Id)
            .IsRequired()
            .HasConversion(idConverter);

        builder.Property(i => i.AccountId)
            .IsRequired()
            .HasColumnName("account_id")
            .HasConversion(idConverter);

        builder.Property(i => i.TotalInstallments)
            .IsRequired()
            .HasColumnName("total_installments");

        builder.Property(i => i.TotalAmount)
            .IsRequired()
            .HasColumnName("total_amount")
            .HasPrecision(18, 2)
            .ValueGeneratedNever();

        builder.Property(i => i.InterestRate)
            .IsRequired()
            .HasColumnName("interest_rate")
            .HasPrecision(5, 2)
            .ValueGeneratedNever();

        builder.Property(i => i.FirstPaymentDate)
            .IsRequired()
            .HasColumnName("first_payment_date")
            .ValueGeneratedNever();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasColumnName("status")
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
