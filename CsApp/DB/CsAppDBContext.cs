using Microsoft.EntityFrameworkCore;
using CsApp.DB.Models;
using Microsoft.Extensions.Hosting;

namespace CsApp.DB
{
    public class CsAppDBContext : DbContext
    {
        // SearchPath is Schema
        public static readonly string CONNECTION_STRINGS =
            "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=password;SearchPath=public;";
        public DbSet<Values> Values { get; set; }
        public DbSet<Models.Results> Results { get; set; }

        public CsAppDBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(CONNECTION_STRINGS);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Results>()
                .HasIndex(u => u.filename)
                .IsUnique();
            modelBuilder.Entity<Values>()
            .HasOne(u => u.result)
            .WithMany(c => c.values)
            .HasForeignKey(u => u.result_id);
        }
    }
}
