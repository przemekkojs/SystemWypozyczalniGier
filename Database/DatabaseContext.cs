using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Tables;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<GameCategory> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            modelBuilder.Entity<GameCategory>()
                .HasKey(gc => new { gc.GameId, gc.Category });
            modelBuilder.Entity<Rental>()
                .HasKey(r => new { r.AccountEmail, r.GameId });
        }
    }

}
