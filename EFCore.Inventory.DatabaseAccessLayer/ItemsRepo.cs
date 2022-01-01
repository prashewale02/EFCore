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

        public List<Item> GetItems()
        {
            var items = _context.Items.Include(x => x.Category)
                                      .AsEnumerable()
                                      .Where(x => !x.IsDeleted)
                                      .OrderBy(x => x.Name)
                                      .ToList();

            return items;
        }

        public List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            var items = _context.Items.Include(x => x.Category)
                                      .Where(x => x.CreatedDate >= minDateValue &&
                                            x.CreatedDate <= maxDateValue)
                                      .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
                                      .ToList();
            return items;
        }

        public List<GetItemsForListingDTO> GetItemsForListingsFromProcedure()
        {
            return _context.ItemsForListing
                        .FromSqlRaw("EXECUTE [dbo].[GetItemsForListing]")
                        .ToList();
        }

        public List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive)
        {
            var isActiveParam = new SqlParameter("IsActive", 1);

            return _context.GetItemsTotalValues
                        .FromSqlRaw("SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParam)
                        .ToList();
        }

        public List<FullItemDetailsDTO> GetItemsWithGenresAndCategories()
        {
            return _context.FullItemDetails
                        .FromSqlRaw("SELECT * FROM [dbo].[ViewFullItemDetails]")
                        .AsEnumerable()
                        .OrderBy(x => x.ItemName).ThenBy(x => x.GenreName)
                        .ThenBy(x => x.Category).ThenBy(x => x.PlayerName)
                        .ToList();
        }

        public int UpsertItem(Item item)
        {
            if (item.Id > 0)
                return UpdateItem(item);

            return CreateItem(item);
        }

        public void UsertItems(List<Item> items)
        {
            using (var scope = new TransactionScope (
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                })
            )
            {
                try
                {
                    foreach (var item in items)
                    {
                        bool success = UpsertItem(item) > 0;
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

        public void DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            if (item is null) return;
            item.IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeleteItems(List<int> itemIds)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                })
            )
            {
                try
                {
                    foreach (var itemId in itemIds)
                    {
                        DeleteItem(itemId);
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

        private int CreateItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();

            var newItem = _context.Items.ToList()
                                .FirstOrDefault(x => 
                                    x.Name.ToLower().Equals(item.Name.ToLower()));

            if (newItem is null)
                throw new Exception("Could not create the item as expected.");

            return newItem.Id;
        }

        private int UpdateItem(Item item)
        {
            var dbItem = _context.Items.Include(x => x.Category)
                                       .Include(x => x.Genres)
                                       .Include(x => x.Players)
                                       .FirstOrDefault(x => x.Id == item.Id);
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

            _context.SaveChanges();
            return item.Id;
        }

        #endregion

    }
}