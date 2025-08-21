using Microsoft.EntityFrameworkCore;
using EFCore_Inheritance_Demo_Main9.Models;

namespace EFCore_Inheritance_Demo_Main9.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TPTCar> TPTCars { get; set; }
        //public DbSet<TPTCarModel> TPTCarModels { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Table per type pattern (metode 2)
        //    // Hvis ikke linjerne herunder er medtaget, laves der TPH med kun én tabel i stedet for
        //    modelBuilder.Entity<TPTCar>().ToTable("TPTCars");
        //    modelBuilder.Entity<TPTCarModel>().ToTable("TPTCarModels");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Hvis ikke linjen herunder er med, vil EntityFramework anvende TPH =>
            // Table Per Hierachy metoden => kun én tabel.
            modelBuilder.Entity<TPTCar>().UseTptMappingStrategy();
            modelBuilder.Entity<TPTCarModel>(); // EF Core vil automatisk opdage, at denne nedarver fra Car
        }
    }
}
