
using System.ComponentModel.DataAnnotations;

namespace EFCore.InventoryModels
{
    public class Genre : FullAuditModel
    {
        [StringLength(InventoryModelsConstants.MAX_GENRENAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        public virtual List<ItemGenre> Items { get; set; } = new List<ItemGenre>();
    }
}
