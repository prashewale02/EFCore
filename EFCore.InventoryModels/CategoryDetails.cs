using EFCore.InventoryModels.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.InventoryModels
{
    public class CategoryDetails : IIdentityModel
    {
        [Required]
        [Key, ForeignKey("Category")]
        public int Id { get; set; }

        [Required]
        [StringLength(InventoryModelsConstants.MAX_COLORVALUE_LENGTH)]
        public string ColorValue { get; set; } = string.Empty;

        [Required]
        [StringLength(InventoryModelsConstants.MAX_COLORNAME_LENGTH)]
        public string ColorName { get; set; } = string.Empty;

        public virtual Category? Category { get; set; }
    }
}
