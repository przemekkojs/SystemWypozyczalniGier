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

            modelBuilder.Entity<Game>().HasData(
                new Game { GameId = 5, 
                            PublisherId = 1, 
                            Title = "Tetris", 
                            Description = "Komputerowa gra logiczna stworzona przez Aleksieja Pażytnowa i jego współpracowników, Dimitrija Pawłowskiego i Wadima Geriasimowa. Pojawiła się na rynku po raz pierwszy 6 czerwca 1984 roku w Związku Radzieckim.",
                            Price = 19.99,
                            QuantityInStock = 5,
                            MaxQuantity = 5,
                            Accessibility = Accessibility.ACCESSIBLE,
                            Discount = 0,
                            Pegi = Pegi.THREE,
                            MainPhotoName = "5_0.png",
                            Photo1Name = "5_1.png" }
            );
        }
    }

}
