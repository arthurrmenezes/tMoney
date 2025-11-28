using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(c => c.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        #region Properties Configuration

        builder.Property(c => c.Id)
            .IsRequired()
            .HasConversion(
                c => c.Id,
                c => IdValueObject.Factory(c));

        builder.Property(c => c.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(80)
            .ValueGeneratedNever();

        builder.Property(c => c.Type)
            .IsRequired()
            .HasColumnName("type")
            .ValueGeneratedNever();

        var accountIdConverter = new ValueConverter<IdValueObject, Guid>(
            a => a.Id,
            a => IdValueObject.Factory(a));

        builder.Property(c => c.AccountId)
            .IsRequired()
            .HasColumnName("account_id")
            .HasConversion(accountIdConverter);

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at")
            .ValueGeneratedNever();

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        #endregion
    }
}
