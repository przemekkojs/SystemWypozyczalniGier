using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o wydawcach gier.
    /// </summary>
    public class Publishers
    {
        [Key]
        [Required]
        [MaxLength(50, ErrorMessage = "Nazwa wydawcy jest zbyt długa.")]
        [NotNull]
        public string Name { get; set; }

        [MaxLength(2000, ErrorMessage = "Opis wydawcy jest zbyt długi")]
        public string? Description { get; set; }


        [InverseProperty(nameof(Tables.Games.Publisher))]
        public virtual ICollection<Games> Games { get; set; }
    }
}
