using Microsoft.EntityFrameworkCore;

namespace FootballApi.Models
{
    public class FootballDBContext : DbContext
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=FootballDb;Trusted_Connection=True;";
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>(mb =>
            {
                mb.Property(c => c.Name).IsRequired();
                mb.Property(c => c.City).IsRequired();
                mb.Property(c => c.Country).IsRequired();
            });
            modelBuilder.Entity<Player>(mb =>
            {
                mb.Property(p => p.Name).IsRequired().HasMaxLength(30);
                mb.Property(p => p.Country).IsRequired();
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
