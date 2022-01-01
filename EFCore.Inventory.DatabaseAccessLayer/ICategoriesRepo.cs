using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public interface ICategoriesRepo
    {
        List<CategoryDTO> ListCategoriesAndDetails();
    }
}
