using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Auth.Entities;

namespace tMoney.Infrastructure.Data.Mappings;

public sealed class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Token)
            .IsUnique();

        #region Properties Configuration

        builder.Property(r => r.Id)
            .IsRequired()
            .HasConversion(
                r => r.Id,
                r => IdValueObject.Factory(r));

        builder.Property(r => r.Token)
            .IsRequired()
            .HasColumnName("token")
            .ValueGeneratedNever();

        builder.Property(r => r.UserId)
            .IsRequired()
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(r => r.ReplacedByToken)
            .IsRequired(false)
            .HasColumnName("replaced_by_token")
            .ValueGeneratedNever();

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        builder.Property(r => r.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at")
            .ValueGeneratedNever();

        builder.Property(r => r.RevokedAt)
            .IsRequired(false)
            .HasColumnName("revoked_at")
            .ValueGeneratedNever();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}
