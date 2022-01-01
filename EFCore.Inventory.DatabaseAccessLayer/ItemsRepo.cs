using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Models;
using EFCore.Inventory.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Transactions;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public class ItemsRepo : IItemsRepo
    {
        #region Private Members 

        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for defining Dbcontext and Mapper.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ItemsRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        public async Task<List<Item>> GetItems()
        {
           var items = await _context.Items.Include(x => x.Category)
                                      //.AsEnumerable()
                                      .Where(x => !x.IsDeleted)
                                      .ToListAsync();

            return items.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<ItemDTO>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            return await _context.Items.Include(x => x.Category)
                                      .Where(x => x.CreatedDate >= minDateValue &&
                                            x.CreatedDate <= maxDateValue)
                                      .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
                                      .ToListAsync();
        }

        public async Task<List<GetItemsForListingDTO>> GetItemsForListingsFromProcedure()
        {
            return await _context.ItemsForListing
                        .FromSqlRaw("EXECUTE [dbo].[GetItemsForListing]")
                        .ToListAsync();
        }

        public async Task<List<GetItemsTotalValueDTO>> GetItemsTotalValues(bool isActive)
        {
            var isActiveParam = new SqlParameter("IsActive", 1);

            return await _context.GetItemsTotalValues
                            .FromSqlRaw(
                                "SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)",
                                isActiveParam)
                            .ToListAsync();
        }

        public async Task<List<FullItemDetailsDTO>> GetItemsWithGenresAndCategories()
        {
            var result = await _context.FullItemDetails
                                    .FromSqlRaw(
                                        "SELECT * FROM [dbo].[ViewFullItemDetails]")
                                    //.AsEnumerable()
                                    .ToListAsync();
            return result.OrderBy(x => x.ItemName).ThenBy(x => x.GenreName)
                         .ThenBy(x => x.Category).ThenBy(x => x.PlayerName)
                         .ToList();
       }

        public async Task<int> UpsertItem(Item item)
        {
            if (item.Id > 0)
                return await UpdateItem(item);

            return await CreateItem(item);
        }

        public async Task UsertItems(List<Item> items)
        {
            using (var scope = new TransactionScope (
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled)
            )
            {
                try
                {
                    foreach (var item in items)
                    {
                        bool success = await UpsertItem(item) > 0;
                        if (success is false)
                            throw new Exception(
                                $"Error while saving the item {item.Name}");
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        public async Task DeleteItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;
            item.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItems(List<int> itemIds)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled)
            )
            {
                try
                {
                    foreach (var itemId in itemIds)
                    {
                        await DeleteItem(itemId);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    throw; // Make sure it is know that the transaction failed.
                }
            }
        }

        #endregion

        #region Private Heleper Methods

        private async Task<int> CreateItem(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            //var items = await _context.Items.ToListAsync();
            //var newItem = items.FirstOrDefault(x =>
            //                        x.Name.ToLower().Equals(item.Name.ToLower()));

            if (item.Id <= 0)
                throw new Exception("Could not create the item as expected.");

            return item.Id;
        }

        private async Task<int> UpdateItem(Item item)
        {
            var dbItem = await _context.Items
                                       .Include(x => x.Category)
                                       .Include(x => x.Genres)
                                       .Include(x => x.Players)
                                       .FirstOrDefaultAsync(x => x.Id == item.Id);
            if (dbItem is null)
                throw new Exception("Item not found!");

            dbItem.CategoryId = item.CategoryId;
            dbItem.CurrentOrFinalPrice = item.CurrentOrFinalPrice;
            dbItem.Description = item.Description;
            dbItem.IsActive = item.IsActive;
            dbItem.IsDeleted = item.IsDeleted;
            dbItem.IsOnSale = item.IsOnSale;
            dbItem.Genres = item.Genres ?? dbItem.Genres;
            dbItem.Name = item.Name;
            dbItem.Notes = item.Notes;
            dbItem.Players = item.Players ?? dbItem.Players;
            dbItem.PurchaseDate = item.PurchaseDate;
            dbItem.PurchasePrice = item.PurchasePrice;
            dbItem.Quantity = item.Quantity;
            dbItem.SoldDate = item.SoldDate;

            await _context.SaveChangesAsync();
            return item.Id;
        }

        #endregion

    }
}