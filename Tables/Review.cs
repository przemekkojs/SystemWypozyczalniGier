using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o recenzjach gier.
    /// </summary>
    public class Review
    {
        [Key]
        [Required]
        [NotNull]
        public int ReviewId { get; set; }

        [Required]
        [NotNull]
        public int GameId { get; set; }
        [ForeignKey(nameof(GameId))]        
        public virtual Game? Game { get; set; }

        [Required]
        [Range(1, 5)]
        public int Mark { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }

        public Review() { }
        public Review(int reviewId, int gameId, int mark, string? comment)
        {
            ReviewId = reviewId;
            GameId = gameId;
            Mark = mark;
            Comment = comment;
        }
    }
}
