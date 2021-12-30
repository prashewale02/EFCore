using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.InventoryModels
{
    public class Item : FullAuditModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        [Range(InventoryModelsConstants.MIN_QUANTITY,
            InventoryModelsConstants.MAX_QUANTITY)]
        public int Quantity { get; set; }

        [StringLength(InventoryModelsConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }

        [StringLength(InventoryModelsConstants.MAX_NOTES_LENGTH,
            MinimumLength = 10)]
        public string? Notes { get; set; }

        public bool IsOnSale { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? SoldDate { get; set; }

        [Range(InventoryModelsConstants.MIN_PRICE,
            InventoryModelsConstants.MAX_PRICE)]
        public decimal? PurchasePrice { get; set; }

        [Range(InventoryModelsConstants.MIN_PRICE, 
            InventoryModelsConstants.MAX_PRICE)]
        public decimal? CurrentOrFinalPrice { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual List<Player> Players { get; set; } = new List<Player>();
        public virtual List<ItemGenre> Genres { get; set; } = new List<ItemGenre>();

    }
}