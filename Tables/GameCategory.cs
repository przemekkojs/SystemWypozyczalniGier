using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o kategoriach gier
    /// </summary>
    public class GameCategory
    {
        [Key]        
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game? Game { get; set; }

        [Key]
        public Category Category { get; set; }

        public GameCategory() { }
        public GameCategory(int gameId, Category category)
        {
            GameId = gameId;
            Category = category;
        }
    }
}
 