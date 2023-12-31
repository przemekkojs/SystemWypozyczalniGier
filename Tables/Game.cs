﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Tables
{
    /// <summary>
    /// Tabela zawierająca informacje o grach.
    /// </summary>
    public class Game
    {
        [Required]
        [NotNull]
        public string MainPhotoName { get; set; }

        [Required]
        [NotNull]
        public string Photo1Name { get; set; }

        [AllowNull]
        public string? Photo2Name { get; set; }

        [AllowNull]
        public string? Photo3Name { get; set; }

        [AllowNull]
        public string? Photo4Name { get; set; }

        [Key]
        [Required]
        [NotNull]
        public int GameId { get; set; }

        [Required]
        public int PublisherId { get; set; }
        [ForeignKey(nameof(PublisherId))]
        public virtual Publisher? Publisher { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Tytuł gry jest za długi.")]
        public string Title { get; set; } = "";

        [Required]
        [MaxLength(2000, ErrorMessage = "Opis gry jest za długi.")]
        public string Description { get; set; } = "";

        [Required]
        [Range(0, 999.99, ErrorMessage = "Cena musi być z zakresu [0, 999.99].")]
        public double Price { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public int MaxQuantity { get; set; }

        [Required]
        public Accessibility Accessibility { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Przecena musi być z zakresu [0, 1].")]
        public double Discount { get; set; } = 0;

        [Required]
        public Pegi Pegi { get; set; }

        [InverseProperty(nameof(Review.Game))]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        [InverseProperty(nameof(Rental.Game))]
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

        [InverseProperty(nameof(GameCategory.Game))]
        public virtual ICollection<GameCategory> Categories { get; set; } = new List<GameCategory>();

        public bool IsDiscounted => Discount > 0;
        public double DiscountedPrice => Price * (1f - Discount);

        public string ShortDescription => 
            Description.Length > 50 
                ? $"{Description[..47]}..." 
                : Description;

        public Game() { }
        public Game(
            int gameId,
            int publisherId,
            string title,
            string description,
            float price,
            int quantityInStock,
            int maxQuantity,
            Accessibility accessibility,
            double discount,
            Pegi pegi, 
            string mainPhotoName,
            string photo1Name,
            string? photo2Name = null,
            string? photo3Name = null,
            string? photo4Name = null)
        {
            GameId = gameId;
            PublisherId = publisherId;
            Title = title;
            Description = description;
            Price = price;
            QuantityInStock = quantityInStock;
            MaxQuantity = maxQuantity;
            Accessibility = accessibility;
            Discount = discount;
            Pegi = pegi;
            MainPhotoName = mainPhotoName;
            Photo1Name = photo1Name;
            Photo2Name = photo2Name;
            Photo3Name = photo3Name;
            Photo4Name = photo4Name;

        }
    }
}
