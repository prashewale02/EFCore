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

        #endregion

        #region Public Methods

        public string GetAllItemsPipeDelimitedString()
        {
            var items = GetItems();
            return String.Join(" | ", items);
        }

        public List<ItemDTO> GetItems() 
            => _mapper.Map<List<ItemDTO>>(_dbRepo.GetItems());

        public List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue) 
            => _dbRepo.GetItemsByDateRange(minDateValue, maxDateValue);

        public List<GetItemsForListingDTO> GetItemsForListingFromProcedure() 
            => _dbRepo.GetItemsForListingsFromProcedure();

        public List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive) 
            => _dbRepo.GetItemsTotalValues(isActive);

        public List<FullItemDetailsDTO> GetItemsWithGenresAndCategories() 
            => _dbRepo.GetItemsWithGenresAndCategories();

        public int UpsertItem(CreateOrUpdateItemDTO item)
        {
            if(item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category id before insert or update");
            }
            return _dbRepo.UpsertItem(_mapper.Map<Item>(item));
        }

        public void UpsertItems(List<CreateOrUpdateItemDTO> items)
        {
            try
            {
                _dbRepo.UsertItems(_mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                // TODO: better logging / not squelching
                Console.WriteLine($"The transaction has failed: {ex.Message}");
            }
        }

        public void DeleteItem(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Please set a valid item id before deleting.");

            _dbRepo.DeleteItem(id);
        }

        public void DeleteItems(List<int> itemIds)
        {
            try
            {
                _dbRepo.DeleteItems(itemIds);
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