using EFCore.Inventory.Models;
using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public interface IItemsRepo
    {
        Task<List<Item>> GetItems();
        Task<List<ItemDTO>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        Task<List<GetItemsForListingDTO>> GetItemsForListingsFromProcedure();
        Task<List<GetItemsTotalValueDTO>> GetItemsTotalValues(bool isActive);
        Task<List<FullItemDetailsDTO>> GetItemsWithGenresAndCategories();
        Task<int> UpsertItem(Item item);
        Task UsertItems(List<Item> items);
        Task DeleteItem(int id);
        Task DeleteItems(List<int> itemIds);
    }
}
