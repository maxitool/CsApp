using Microsoft.EntityFrameworkCore;
using CsApp.DB.Models;

namespace CsApp.DB
{
    public class CsAppDBContext : DbContext
    {
        public DbSet<Values> Values { get; set; }
        public DbSet<Models.Results> Results { get; set; }
        public DbSet<Models.Files> Files { get; set; }

        public CsAppDBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=2403;SearchPath=public;");
        }
    }
}
