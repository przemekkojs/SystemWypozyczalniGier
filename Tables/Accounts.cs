using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o koncie użytkownika.
    /// </summary>
    public class Accounts
    {
        [Key]
        [MaxLength(255)]
        [NotNull]
        [Required]
        public string Email { get; set; }

        [MaxLength(50)]
        [NotNull]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [NotNull]
        [Required]
        public string Surname { get; set; }

        [MaxLength(9)]
        [NotNull]
        [Required]
        public string PhoneNumber { get; set; }

        [MaxLength(32)]
        [NotNull]
        [Required]
        public string Password { get; set; }

        [Required]
        [NotNull]
        public Role Role { get; set; }


        [InverseProperty(nameof(Tables.Rentals.Account))]
        public virtual ICollection<Rentals> Rentals { get; set; }
    }
}
