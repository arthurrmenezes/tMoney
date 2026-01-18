using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class CreditCardInvoiceMapping : IEntityTypeConfiguration<CreditCardInvoice>
{
    public void Configure(EntityTypeBuilder<CreditCardInvoice> builder)
    {
        builder.ToTable("credit_card_invoices");

        builder.HasKey(i => i.Id);

        builder.HasOne<CreditCard>()
            .WithMany()
            .HasForeignKey(i => i.CreditCardId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        var idConverter = new ValueConverter<IdValueObject, Guid>(
            i => i.Id,
            i => IdValueObject.Factory(i));

        #region Properties Configuration

        builder.Property(i => i.Id)
            .IsRequired()
            .HasConversion(idConverter);

        builder.Property(i => i.CreditCardId)
            .IsRequired()
            .HasColumnName("credit_card_id")
            .HasConversion(idConverter);

        builder.Property(i => i.Month)
            .IsRequired()
            .HasColumnName("month")
            .ValueGeneratedNever();

        builder.Property(i => i.Year)
            .IsRequired()
            .HasColumnName("year")
            .ValueGeneratedNever();

        builder.Property(i => i.CloseDay)
            .IsRequired()
            .HasColumnName("close_day")
            .ValueGeneratedNever();

        builder.Property(i => i.DueDay)
            .IsRequired()
            .HasColumnName("due_day")
            .ValueGeneratedNever();

        builder.Property(i => i.AmountPaid)
            .IsRequired()
            .HasColumnName("amount_paid")
            .HasPrecision(18, 2)
            .ValueGeneratedNever();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasColumnName("status")
            .ValueGeneratedNever();

        builder.Property(i => i.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at")
            .ValueGeneratedNever();

        builder.Property(i => i.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        #endregion
    }
}
