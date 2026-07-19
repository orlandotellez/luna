using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luna.Domain.Entities;

namespace Luna.Infrastructure.Persistence.Configurations;

public class SymptomEntryConfiguration : IEntityTypeConfiguration<SymptomEntry>
{
    public void Configure(EntityTypeBuilder<SymptomEntry> builder)
    {
        builder.ToTable("SymptomEntries");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.UserId).IsRequired().HasColumnName("user_id");
        builder.HasIndex(s => s.UserId);

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.Date).IsRequired().HasColumnName("date");
        builder.Property(s => s.Symptom).IsRequired().HasColumnName("symptom").HasMaxLength(100);
        builder.Property(s => s.Severity).HasColumnName("severity");
        builder.Property(s => s.Notes).HasColumnName("notes").HasMaxLength(500);

        builder.Property(s => s.CreatedAt).IsRequired().HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(s => s.UpdatedAt).IsRequired().HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
