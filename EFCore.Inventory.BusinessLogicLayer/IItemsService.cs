using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public interface IItemsService
    {
        List<ItemDTO> GetItems();
        List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        List<GetItemsForListingDTO> GetItemsForListingFromProcedure();
        List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive);
        string GetAllItemsPipeDelimitedString();
        List<FullItemDetailsDTO> GetItemsWithGenresAndCategories();
        int UpsertItem(CreateOrUpdateItemDTO item);
        void UpsertItems(List<CreateOrUpdateItemDTO> items);
        void DeleteItem(int id);
        void DeleteItems(List<int> itemIds);
    }
}
