using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public interface ICategoriesRepo
    {
        Task<List<CategoryDTO>> ListCategoriesAndDetails();
    }
}
