using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Tables;
using SystemWypozyczalniGier.Enumerations;
using SystemWypozyczalniGier.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace SystemWypozyczalniGier.Database
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<GameCategory> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            modelBuilder.Entity<GameCategory>()
                .HasKey(gc => new { gc.GameId, gc.Category });
            modelBuilder.Entity<Rental>()
                .HasKey(r => new { r.AccountEmail, r.GameId });
        }
    }

}
