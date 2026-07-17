using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luna.Domain.Entities;

namespace Luna.Infrastructure.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.UserId).IsRequired().HasColumnName("user_id");
        builder.HasIndex(p => p.UserId).IsUnique(); // 1:1

        builder.HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.UserName).HasColumnName("username").HasMaxLength(50);
        builder.HasIndex(p => p.UserName).IsUnique();

        builder.Property(p => p.Bio).HasColumnName("bio");
        builder.Property(p => p.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(p => p.DepartmentId).HasColumnName("department_id").HasMaxLength(10);
        builder.Property(p => p.MunicipalityId).HasColumnName("municipality_id").HasMaxLength(10);

        builder.Property(p => p.CreatedAt).IsRequired().HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(p => p.UpdatedAt).IsRequired().HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
