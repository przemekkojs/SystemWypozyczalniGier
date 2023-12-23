using SystemWypozyczalniGier.Enumerations;
using SystemWypozyczalniGier.Tables;

namespace SystemWypozyczalniGier.Models
{
    public class GameViewModel
    {
        public IEnumerable<Game> Games { get; set; }

        public IEnumerable<Category> FilterCategories { get; set; }

        public static IEnumerable<Category> AllCategories { get; private set; } =
            Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
    }
}
