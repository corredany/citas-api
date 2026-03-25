namespace CitasApi.Infrastructure.Database;

using CitasApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cita> Citas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Estado).HasDefaultValue("pendiente");
            entity.Property(c => c.CreadoEn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(c => c.ActualizadoEn).HasDefaultValueSql("GETUTCDATE()");
            entity.HasOne(c => c.Cliente)
                  .WithMany()
                  .HasForeignKey(c => c.ClienteId);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.CreadoEn).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(c => c.Email).IsUnique();
        });
    }
}