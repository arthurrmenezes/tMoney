using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class InstallmentItemMapping : IEntityTypeConfiguration<InstallmentItem>
{
    public void Configure(EntityTypeBuilder<InstallmentItem> builder)
    {
        builder.ToTable("installment_items");

        builder.HasKey(i => i.Id);

        builder.HasOne<CreditCardInvoice>()
            .WithMany()
            .HasForeignKey(i => i.InvoiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        var idConverter = new ValueConverter<IdValueObject, Guid>(
            i => i.Id,
            i => IdValueObject.Factory(i));

        #region Properties Configuration

        builder.Property(i => i.Id)
            .IsRequired()
            .HasConversion(idConverter);

        builder.Property(i => i.InstallmentId)
            .IsRequired()
            .HasColumnName("installment_id")
            .HasConversion(idConverter);

        builder.Property(i => i.InvoiceId)
            .IsRequired(false)
            .HasColumnName("invoice_id")
            .HasConversion(idConverter);

        builder.Property(i => i.Number)
            .IsRequired()
            .HasColumnName("number")
            .ValueGeneratedNever();

        builder.Property(i => i.Amount)
            .IsRequired()
            .HasColumnName("amount")
            .HasPrecision(18,2)
            .ValueGeneratedNever();

        builder.Property(i => i.DueDate)
            .IsRequired()
            .HasColumnName("due_date")
            .ValueGeneratedNever();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasColumnName("status")
            .ValueGeneratedNever();

        builder.Property(i => i.PaidAt)
            .IsRequired(false)
            .HasColumnName("paid_at")
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
