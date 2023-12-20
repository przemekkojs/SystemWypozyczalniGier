using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o koncie użytkownika.
    /// </summary>
    public class Account
    {
        [Key]
        [MaxLength(255)]
        [NotNull]
        [Required]
        public string Email { get; set; } = "";

        [MaxLength(50)]
        [NotNull]
        [Required]
        public string Name { get; set; } = "";

        [MaxLength(50)]
        [NotNull]
        [Required]
        public string Surname { get; set; } = "";

        [MaxLength(9)]
        [NotNull]
        [Required]
        public string PhoneNumber { get; set; } = "";

        [MaxLength(32)]
        [NotNull]
        [Required]
        public string Password { get; set; } = "";

        [Required]
        [NotNull]
        public Role Role { get; set; } = Role.CLIENT;

        [InverseProperty(nameof(Rental.Account))]
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

        public Account() { }
        public Account(
            string email,
            string name,
            string surname,
            string phoneNumber,
            string password,
            Role role)
        {
            Email = email;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
        }
    }
}
