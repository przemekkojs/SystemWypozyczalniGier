using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o wydawcach gier.
    /// </summary>
    public class Publisher
    {
        [Key]
        [Required]
        [NotNull]
        public int PublisherId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Nazwa wydawcy jest zbyt długa.")]
        [NotNull]
        public string Name { get; set; } = "";

        [MaxLength(2000, ErrorMessage = "Opis wydawcy jest zbyt długi")]
        public string? Description { get; set; }

        [InverseProperty(nameof(Game.Publisher))]
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();

        public Publisher() { }
        public Publisher(int publisherId, string name, string? description)
        {
            PublisherId = publisherId;
            Name = name;
            Description = description;
        }
    }
}
