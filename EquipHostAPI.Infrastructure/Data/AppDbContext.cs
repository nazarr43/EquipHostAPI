using EquipHostAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipHostAPI.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public DbSet<ProductionFacility> ProductionFacilities { get; set; } 
    public DbSet<EquipmentType> EquipmentTypes { get; set; } 
    public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductionFacility>()
            .Property(p => p.StandardArea)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<EquipmentType>()
            .Property(e => e.Area)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ProductionFacility>().HasData(
            new ProductionFacility { Id = 1, Code = "PF01", Name = "Facility A", StandardArea = 500 },
            new ProductionFacility { Id = 2, Code = "PF02", Name = "Facility B", StandardArea = 1000 }
        );

        modelBuilder.Entity<EquipmentType>().HasData(
            new EquipmentType { Id = 1, Code = "ET01", Name = "Type A", Area = 50 },
            new EquipmentType { Id = 2, Code = "ET02", Name = "Type B", Area = 100 }
        );
    }

}

