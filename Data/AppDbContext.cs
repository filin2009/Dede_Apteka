using Dede_Apteka.Models;
using Microsoft.EntityFrameworkCore;

namespace Dede_Apteka.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Drug> Drugs => Set<Drug>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Benchmark> Benchmarks => Set<Benchmark>();
    public DbSet<Dede> Dede => Set<Dede>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // Имена таблиц сопоставляем со схемой, которую создаёт Evolve (snake_case).
        b.Entity<Drug>(e =>
        {
            e.ToTable("drugs");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Manufacturer).HasColumnName("manufacturer");
            e.Property(x => x.Price).HasColumnName("price");
            e.Property(x => x.Quantity).HasColumnName("quantity");
        });

        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Username).HasColumnName("username");
            e.Property(x => x.PasswordHash).HasColumnName("password_hash");
            e.Property(x => x.Role).HasColumnName("role");
        });

        b.Entity<Benchmark>(e =>
        {
            e.ToTable("benchmark");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Message).HasColumnName("message");
            e.Property(x => x.InsertTime).HasColumnName("insert_time");
        });

        b.Entity<Dede>(e =>
        {
            e.ToTable("dede");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
        });

        b.Entity<Reservation>(e =>
        {
            e.ToTable("reservations");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.CustomerName).HasColumnName("customer_name");
            e.Property(x => x.ReservationDate).HasColumnName("reservation_date");
        });
    }
}
