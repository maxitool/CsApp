using Microsoft.EntityFrameworkCore;
using CsApp.DB.Models;

namespace CsApp.DB
{
    public class CsAppDBContext : DbContext
    {
        public static readonly string HOST = "localhost";
        public static readonly int PORT = 5432;
        public static readonly string DATABASE = "postgres";
        public static readonly string SCHEMA = "public";
        public static readonly string USERNAME = "postgres";
        public static readonly string PASSWORD = "2403";
        public static readonly string CONNECTION_STRINGS =
            $"Host={HOST};Port={PORT};Database={DATABASE};Username={USERNAME};Password={PASSWORD};SearchPath={SCHEMA};";
        public DbSet<Values> Values { get; set; }
        public DbSet<Models.Results> Results { get; set; }
        public DbSet<Models.Files> Files { get; set; }

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
            
            modelBuilder.Entity<Files>()
                .HasIndex(u => u.filename)
                .IsUnique();
        }
    }
}
