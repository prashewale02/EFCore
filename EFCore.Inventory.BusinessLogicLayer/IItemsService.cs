using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public interface IItemsService
    {
        Task<List<ItemDTO>> GetItems();
        Task<List<ItemDTO>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        Task<List<GetItemsForListingDTO>> GetItemsForListingFromProcedure();
        Task<List<GetItemsTotalValueDTO>> GetItemsTotalValues(bool isActive);
        Task<string> GetAllItemsPipeDelimitedString();
        Task<List<FullItemDetailsDTO>> GetItemsWithGenresAndCategories();
        Task<int> UpsertItem(CreateOrUpdateItemDTO item);
        Task UpsertItems(List<CreateOrUpdateItemDTO> items);
        Task DeleteItem(int id);
        Task DeleteItems(List<int> itemIds);
    }
}
