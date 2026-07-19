using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luna.Domain.Entities.Cycle;

namespace Luna.Infrastructure.Persistence.Configurations.Cycle;

public class PeriodEntryConfiguration : IEntityTypeConfiguration<PeriodEntry>
{
    public void Configure(EntityTypeBuilder<PeriodEntry> builder)
    {
        builder.ToTable("PeriodEntries");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.UserId).IsRequired().HasColumnName("user_id");
        builder.HasIndex(p => p.UserId);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.StartDate).IsRequired().HasColumnName("start_date");
        builder.Property(p => p.EndDate).HasColumnName("end_date");
        builder.Property(p => p.Notes).HasColumnName("notes").HasMaxLength(500);

        builder.Property(p => p.CreatedAt).IsRequired().HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(p => p.UpdatedAt).IsRequired().HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
