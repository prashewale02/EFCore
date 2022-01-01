using AutoMapper;
using EFCore.Inventory.DatabaseAccessLayer;
using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Models;
using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public class ItemsService : IItemsService
    {
        #region Private Members 

        private readonly IItemsRepo _dbRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ItemsService(InventoryDbContext context, IMapper mapper)
        {
            _dbRepo = new ItemsRepo(context, mapper);
            _mapper = mapper;
        }

        public ItemsService(IItemsRepo dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        public async Task<string> GetAllItemsPipeDelimitedString()
        {
            var items = await GetItems();
            return String.Join(" | ", items);
        }

        public async Task<List<ItemDTO>> GetItems() 
            => _mapper.Map<List<ItemDTO>>(await _dbRepo.GetItems());

        public async Task<List<ItemDTO>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue) 
            => await _dbRepo.GetItemsByDateRange(minDateValue, maxDateValue);

        public async Task<List<GetItemsForListingDTO>> GetItemsForListingFromProcedure() 
            => await _dbRepo.GetItemsForListingsFromProcedure();

        public async Task<List<GetItemsTotalValueDTO>> GetItemsTotalValues(bool isActive) 
            => await _dbRepo.GetItemsTotalValues(isActive);

        public async Task<List<FullItemDetailsDTO>> GetItemsWithGenresAndCategories() 
            => await _dbRepo.GetItemsWithGenresAndCategories();

        public async Task<int> UpsertItem(CreateOrUpdateItemDTO item)
        {
            if(item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category id before insert or update");
            }
            return await _dbRepo.UpsertItem(_mapper.Map<Item>(item));
        }

        public async Task UpsertItems(List<CreateOrUpdateItemDTO> items)
        {
            try
            {
                await _dbRepo.UsertItems(_mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                // TODO: better logging / not squelching
                Console.WriteLine($"The transaction has failed: {ex.Message}");
            }
        }

        public async Task DeleteItem(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Please set a valid item id before deleting.");

            await _dbRepo.DeleteItem(id);
        }

        public async Task DeleteItems(List<int> itemIds)
        {
            try
            {
                await _dbRepo.DeleteItems(itemIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging / not squelching
                Console.WriteLine($"The transaction has failed: {ex.Message}");
            }
        }

        #endregion
    }
}