using EFCore.Inventory.Models;
using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public interface IItemsRepo
    {
        List<Item> GetItems();
        List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        List<GetItemsForListingDTO> GetItemsForListingsFromProcedure();
        List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive);
        List<FullItemDetailsDTO> GetItemsWithGenresAndCategories();
        int UpsertItem(Item item);
        void UsertItems(List<Item> items);
        void DeleteItem(int id);
        void DeleteItems(List<int> itemIds);
    }
}
