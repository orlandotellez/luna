using Luna.Domain.Entities.Auth;
using Luna.Domain.Entities.Users;
using Luna.Domain.Entities.Cycle;
using Luna.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Luna.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<HealthProfile> HealthProfiles => Set<HealthProfile>();
    public DbSet<Pregnancy> Pregnancies => Set<Pregnancy>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Verification> Verifications => Set<Verification>();
    public DbSet<PeriodEntry> PeriodEntries => Set<PeriodEntry>();
    public DbSet<SymptomEntry> SymptomEntries => Set<SymptomEntry>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PeriodEntryConfiguration());
        modelBuilder.ApplyConfiguration(new SymptomEntryConfiguration());
    }
}
