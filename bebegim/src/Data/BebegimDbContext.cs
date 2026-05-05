namespace bebegim.Data;
using Microsoft.EntityFrameworkCore;
using bebegim.Models;
public class BebegimDbContext : DbContext
{
    public BebegimDbContext(DbContextOptions<BebegimDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Kid> Kids { get; set; }
    public DbSet<GrowHistory> GrowHistories { get; set; }
    public DbSet<SleepHistory> SleepHistories { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<FoodHistory> FoodHistories { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<Illness> Illnesses { get; set; }

    public DbSet<Food> Foods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User-Kid ilişkisi
        modelBuilder.Entity<Kid>()
            .HasOne(k => k.Parent)
            .WithMany()
            .HasForeignKey(k => k.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
