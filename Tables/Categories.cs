using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o kategoriach gier
    /// </summary>
    public class Categories
    {
        [Key]        
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public virtual Games Game { get; set; }

        [Key]
        public Category Category { get; set; }
    }
}
