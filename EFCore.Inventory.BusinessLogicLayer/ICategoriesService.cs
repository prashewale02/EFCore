using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public interface ICategoriesService
    {
        List<CategoryDTO> ListCategoriesAndDetails();
    }
}