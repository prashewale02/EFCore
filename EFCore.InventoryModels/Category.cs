using System.ComponentModel.DataAnnotations;

namespace EFCore.InventoryModels
{
    public class Category : FullAuditModel
    { 
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; } 

        public virtual List<Item> Items { get; set; } = new List<Item>();

        public virtual CategoryDetails CategoryDetails { get; set; }
    }
}
