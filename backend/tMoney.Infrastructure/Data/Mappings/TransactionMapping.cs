using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Card>()
            .WithMany()
            .HasForeignKey(t => t.CardId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Installment>()
            .WithOne()
            .HasForeignKey<Transaction>(t => t.InstallmentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<CreditCardInvoice>()
            .WithMany()
            .HasForeignKey(t => t.InvoiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        var idConverter = new ValueConverter<IdValueObject, Guid>(
            t => t.Id,
            t => IdValueObject.Factory(t));

        #region Index Key Configuration

        builder.HasIndex(t => new { t.AccountId, t.CreatedAt });

        builder.HasIndex(t => new { t.AccountId, t.Status, t.Date });

        builder.HasIndex(t => new { t.Status, t.Date });

        #endregion

        #region Properties Configuration

        builder.Property(t => t.Id)
            .IsRequired()
            .HasConversion(idConverter);

        builder.Property(t => t.AccountId)
            .IsRequired()
            .HasColumnName("account_id")
            .HasConversion(idConverter);

        builder.Property(t => t.CardId)
            .IsRequired()
            .HasColumnName("card_id")
            .HasConversion(idConverter);

        builder.Property(t => t.CategoryId)
            .IsRequired()
            .HasColumnName("category_id")
            .HasConversion(idConverter);

        builder.Property(t => t.InstallmentId)
            .IsRequired(false)
            .HasColumnName("installment_id")
            .HasConversion(
                valueObject => valueObject! != null! ? valueObject.Id : (Guid?)null,
                dbValue => dbValue.HasValue ? IdValueObject.Factory(dbValue.Value) : null);

        builder.Property(t => t.InvoiceId)
            .IsRequired(false)
            .HasColumnName("invoice_id")
            .HasConversion(
                valueObject => valueObject! != null! ? valueObject.Id : (Guid?)null,
                dbValue => dbValue.HasValue ? IdValueObject.Factory(dbValue.Value) : null);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(100)
            .ValueGeneratedNever();

        builder.Property(t => t.Description)
            .IsRequired(false)
            .HasColumnName("description")
            .HasMaxLength(255)
            .ValueGeneratedNever();

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .ValueGeneratedNever();

        builder.Property(t => t.Date)
            .IsRequired()
            .HasColumnName("date")
            .ValueGeneratedNever();

        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasColumnName("transaction_type")
            .ValueGeneratedNever();

        builder.Property(t => t.PaymentMethod)
            .IsRequired()
            .HasColumnName("payment_method")
            .ValueGeneratedNever();

        builder.Property(t => t.Status)
            .IsRequired()
            .HasColumnName("status")
            .ValueGeneratedNever();

        builder.Property(t => t.Destination)
            .IsRequired(false)
            .HasColumnName("destination")
            .ValueGeneratedNever();

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at")
            .ValueGeneratedNever();

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        #endregion
    }
}
