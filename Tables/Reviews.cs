using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o recenzjach gier.
    /// </summary>
    public class Reviews
    {
        [Key]
        [Required]
        [NotNull]
        public int ReviewId { get; set; }

        [Required]
        [NotNull]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]        
        public virtual Games Game { get; set; }

        [Required]
        [Range(0, 10)]
        public int Mark { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
}
