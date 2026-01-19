using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class CardMapping : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");

        builder.HasKey(c => c.Id);

        builder.HasDiscriminator(c => c.Type)
            .HasValue<DebitCard>(CardType.DebitCard)
            .HasValue<CreditCard>(CardType.CreditCard);

        var idConverter = new ValueConverter<IdValueObject, Guid>(
            i => i.Id,
            i => IdValueObject.Factory(i));

        #region Common Properties Configuration

        builder.Property(c => c.Id)
            .IsRequired()
            .HasConversion(idConverter);

        builder.Property(c => c.AccountId)
            .IsRequired()
            .HasColumnName("account_id")
            .HasConversion(idConverter);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(50)
            .ValueGeneratedNever();

        builder.Property(c => c.Type)
            .IsRequired()
            .HasColumnName("card_type")
            .ValueGeneratedNever();

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
