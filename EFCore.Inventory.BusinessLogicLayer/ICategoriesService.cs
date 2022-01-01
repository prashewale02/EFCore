using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public interface ICategoriesService
    {
        Task<List<CategoryDTO>> ListCategoriesAndDetails();
    }
}