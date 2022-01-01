using System.ComponentModel.DataAnnotations;

namespace EFCore.Inventory.Models
{
    public class Category : FullAuditModel
    {
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        public virtual List<Item> Items { get; set; } = new List<Item>();

        public virtual CategoryDetails? CategoryDetails { get; set; }
    }
}
