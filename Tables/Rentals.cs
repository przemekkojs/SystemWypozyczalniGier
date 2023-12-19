using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o wypożyczeniach gier.
    /// </summary>
    public class Rentals
    {
        [Key]
        [Required]
        [NotNull]
        public int AccountId { get; set; }
        
        [ForeignKey(nameof(AccountId))]        
        public virtual Accounts Account { get; set; }

        [Key]
        [Required]
        [NotNull]
        public int GameId { get; set; }
        
        [ForeignKey(nameof(GameId))]        
        public virtual Games Game { get; set; }

        [Required]
        public RentalStatus RentalStatus { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime RentalTime { get; set; }
    }
}
