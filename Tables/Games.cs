using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o grach.
    /// </summary>
    public class Games
    {
        [Key]
        [Required]
        [NotNull]
        public int GameId { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [ForeignKey(nameof(PublisherId))]        
        public virtual Publishers Publisher { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Tytuł gry jest za długi.")]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000, ErrorMessage = "Opis gry jest za długi.")]
        public string Description { get; set; }

        [Required]
        [Range(0, 999.99, ErrorMessage = "Cena musi być z zakresu [0, 999.99].")]
        public double Price { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public int MaxQuantity { get; set; }

        [Required]
        public Accessibility Accessibility { get; set; }

        public double Discount { get; set; }

        [Required]
        public Pegi Pegi { get; set; }


        [InverseProperty(nameof(Tables.Reviews.Game))]
        public virtual ICollection<Reviews> Reviews { get; set; }

        [InverseProperty(nameof(Tables.Rentals.Game))]
        public virtual ICollection<Rentals> Rentals { get; set; }

        [InverseProperty(nameof(Tables.Categories.Category))]
        public virtual ICollection<Categories> Categories { get; set; }
    }
}
