using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luna.Domain.Entities;

namespace Luna.Infrastructure.Persistence.Configurations;

public class PregnancyConfiguration : IEntityTypeConfiguration<Pregnancy>
{
    public void Configure(EntityTypeBuilder<Pregnancy> builder)
    {
        builder.ToTable("Pregnancies");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.UserId).IsRequired().HasColumnName("user_id");
        builder.HasIndex(p => p.UserId);                       // (1:N)

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.LastMenstrualPeriod).HasColumnName("last_menstrual_period");
        builder.Property(p => p.EstimatedDueDate).IsRequired().HasColumnName("estimated_due_date");
        builder.Property(p => p.CurrentWeek).IsRequired().HasColumnName("current_week");
        builder.Property(p => p.IsFirstPregnancy).HasColumnName("is_first_pregnancy");
        builder.Property(p => p.PregnancyCount).HasColumnName("pregnancy_count").HasDefaultValue(1);
        builder.Property(p => p.IsActive).IsRequired().HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(p => p.EndedAt).HasColumnName("ended_at");
        builder.Property(p => p.Notes).HasColumnName("notes");

        builder.Property(p => p.CreatedAt).IsRequired().HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(p => p.UpdatedAt).IsRequired().HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(p => p.IsActive);
    }
}
