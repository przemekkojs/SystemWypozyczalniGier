using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o wypożyczeniach gier.
    /// </summary>
    public class Rental
    {
        [Key]
        [Required]
        [NotNull]
        public string AccountEmail { get; set; } = "";
        [ForeignKey(nameof(AccountEmail))]        
        public virtual Account? Account { get; set; }

        [Key]
        [Required]
        [NotNull]
        public int GameId { get; set; }
        [ForeignKey(nameof(GameId))]        
        public virtual Game? Game { get; set; }

        [Required]
        public RentalStatus RentalStatus { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? RentalTime { get; set; }

        public Rental() { }
        public Rental(string accountEmail, int gameId, RentalStatus status, DateTime? rentalTime)
        {
            AccountEmail = accountEmail;
            GameId = gameId;
            RentalStatus = status;
            RentalTime = rentalTime;
        }
    }
}
