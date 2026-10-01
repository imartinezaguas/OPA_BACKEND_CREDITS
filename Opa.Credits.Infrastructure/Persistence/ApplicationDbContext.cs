using Microsoft.EntityFrameworkCore;
using Opa.Credits.Domain.Entities;

namespace Opa.Credits.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Associate> Associates { get; set; }
    public DbSet<Credit> Credits { get; set; }
    public DbSet<CreditHistory> CreditHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración Associate
        modelBuilder.Entity<Associate>(entity =>
        {
            entity.ToTable("associates");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Identification).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Identification).IsUnique(); // Identificación única por associate
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            
            // Relación 1-a-N con Créditos
            entity.HasMany(a => a.Credits)
                  .WithOne(c => c.Associate)
                  .HasForeignKey(c => c.AssociateId)
                  .OnDelete(DeleteBehavior.Restrict); // No permitir borrar un associate si tiene créditos
        });

        // Configuración Credit
        modelBuilder.Entity<Credit>(entity =>
        {
            entity.ToTable("credits");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreditNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.CreditNumber).IsUnique(); // Regla de duplicidad: Número de crédito único

            entity.Property(e => e.CreditType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            
            // Tipos de datos monetarios exigidos en el PDF
            entity.Property(e => e.RequestedValue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(18,2)");

            // Mapear el Enum como String en la base de datos (más legible que guardar un int)
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
            
            // Relación 1-a-N con Histories
            entity.HasMany(c => c.Histories)
                  .WithOne(h => h.Credit)
                  .HasForeignKey(h => h.CreditId)
                  .OnDelete(DeleteBehavior.Cascade); // Si por alguna razón se borra el crédito, se va su historial
        });

        // Configuración CreditHistory
        modelBuilder.Entity<CreditHistory>(entity =>
        {
            entity.ToTable("credithistories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PreviousStatus).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.NewStatus).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.Observation).HasMaxLength(500);
            entity.Property(e => e.User).HasMaxLength(100);
        });
    }
}
