using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tMoney.Domain.BoundedContexts.CardContext.Entities;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class CreditCardMapping : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        #region Properties Configuration

        builder.Property(c => c.Limit)
            .IsRequired()
            .HasColumnName("limit")
            .HasPrecision(18, 2)
            .ValueGeneratedNever();

        builder.Property(c => c.CloseDay)
            .IsRequired()
            .HasColumnName("close_day")
            .ValueGeneratedNever();

        builder.Property(c => c.DueDay)
            .IsRequired()
            .HasColumnName("due_day")
            .ValueGeneratedNever();

        #endregion
    }
}
