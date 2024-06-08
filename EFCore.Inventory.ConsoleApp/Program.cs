using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using EFCore.Inventory.Models.DTOs;
using AutoMapper.QueryableExtensions;
using EFCore.Inventory.BusinessLogicLayer;
using EFCore.Inventory.Models;

namespace EFCore.Inventory.ConsoleApp
{
    class Program
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;
        private readonly static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder = new();

        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";
        private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";

        private static MapperConfiguration? _mapperConfig;
        private static IMapper? _mapper;
        private static IServiceProvider? _serviceProvider;

        private static IItemsService? _itemsService;
        private static ICategoriesService? _categoriesService;

        private static List<CategoryDTO>? _categories;


        #endregion

        static async Task Main(String[] args)
        {
            Console.WriteLine("***** Welcome to Console App *****");

            BuildOptions();
            _mapper = BuildMapper();

            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                _itemsService = new ItemsService(db, _mapper);
                _categoriesService = new CategoriesService(db, _mapper);

                await ListInventory();
                Console.WriteLine();

                //ListInventoryWithProjection();
                //Console.WriteLine();

                await ListCategoriesAndColors();
                Console.WriteLine();

                await GetItemsForListing();
                Console.WriteLine();

                await GetItemsForListingLinq();
                Console.WriteLine();

                await GetAllActiveItemsAsPipeDelimitedString();
                Console.WriteLine();

                await GetItemsTotalValues();
                Console.WriteLine();

                await GetFullItemDetails();
                Console.WriteLine();

                Console.Write("Would you like to create items? [Yes / No] => ");

                while (Console.ReadLine()?.StartsWith("y",
                        StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Console.WriteLine("\tAdding new Item(s)...");
                    await CreateMultipleItems();
                    Console.WriteLine("\tItems added successfully.");
                    await ListInventory();
                    Console.Write("Would you like to continue making items? [Yes / No] => ");
                }

                Console.Write($"Would you like to update items? [Yes / No] => ");
                
                while(Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Console.WriteLine($"\tUpdating Item(s)...");
                    await UpdateMultipleItems();
                    Console.WriteLine($"\tItems updated successfully.");
                    await ListInventory();
                    Console.Write($"Would you like to continue updating items? [Yes / No] => ");
                }

                Console.Write($"Would you like to delete items? [Yes / No] => ");
                
                while(Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Console.WriteLine($"\tDeleting new Item(s)...");
                    await DeleteMultipleItems();
                    Console.WriteLine($"\tItems updated successfully.");
                    await ListInventory();
                    Console.Write($"Would you like to continue deleting items? [Yes / No] => ");
                }
            }

            Console.WriteLine();
            Console.WriteLine("--> Please press Enter to exit.");
            Console.ReadLine();
        }


        #region Helper Methods

        private static void BuildOptions()
        {
            Console.WriteLine("-> Configuring for connecting to InventoryManager database");
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            //_optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
            Console.WriteLine("\tSuccessfully connected to the database.");
        }

        private static IMapper BuildMapper()
        {
            Console.WriteLine("-> Configuring the AutoMapper Profile.");
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(InventoryMapper));
            _serviceProvider = services.BuildServiceProvider();

            _mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<InventoryMapper>();
            });
            _mapperConfig.AssertConfigurationIsValid();

            Console.WriteLine("\tAutomapper Profile successfully configured.");
            
            return _mapperConfig.CreateMapper();
        }

        private static async Task ListInventory()
        {
            if (_itemsService is null)
                return;

            Console.WriteLine("-> Inventory Items : ");
            var result = await _itemsService.GetItems();
            result.ForEach(x => Console.WriteLine($"\tItem : " +
                $"{x.Name,-40} - {x.Description} has category {x.CategoryName}"));
        }

        //private static void ListInventoryWithProjection()
        //{
        //    Console.WriteLine("-> List inventory with projection: ");
        //    using (var db = new InventoryDbContext(_optionsBuilder.Options))
        //    {
        //        var items =
        //            db.Items
        //                    .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
        //                    .ToList();
        //        items.OrderBy(x => x.Name).ToList().ForEach(x => Console.WriteLine($"\t{x}"));

        //        //items.ForEach(x => Console.WriteLine($"\t{x}"));
        //    }
        //}

        private static async Task GetItemsForListing()
        {
            if (_itemsService is null)
                return;

            Console.WriteLine("-> Getting Items For Listing :");

            var results = await _itemsService.GetItemsForListingFromProcedure();

            foreach (var item in results)
            {
                var output = $"\tItem {item.Name} - {item.Description}";
                if (!string.IsNullOrWhiteSpace(item.CategoryName))
                {
                    output = $"{output} has category : {item.CategoryName}";
                }
                Console.WriteLine(output);
            }
        }

        private static async Task GetItemsForListingLinq() 
        {
            if (_itemsService is null)
                return;

            Console.WriteLine("-> Getting Items for listing using Linq.");
            var minDateValue = new DateTime(2021, 1, 1);
            var maxDateValue = new DateTime(2024, 1, 1);

            var results = await _itemsService.GetItemsByDateRange(minDateValue,
                                                maxDateValue);
            
            foreach (var itemDTO in results.OrderBy(y => y.CategoryName)
                                           .ThenBy(z => z.Name))
            {
                Console.WriteLine($"\t{itemDTO}");
            }
        }

        private static async Task GetAllActiveItemsAsPipeDelimitedString()
        {
            if (_itemsService is null)
                return;

            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                Console.WriteLine($"-> All Active Items: { await _itemsService.GetAllItemsPipeDelimitedString() ?? "No Items Found." }");
            }
        }

        private static async Task GetItemsTotalValues()
        {
            if (_itemsService is null)
                return;

            Console.WriteLine("-> Items Totals : ");
            var results = await _itemsService.GetItemsTotalValues(true);

            Console.WriteLine($"\t{"ID",-10}{"Name",-50}{"Quantity",-15}{"TotalValue",-20}");

            foreach (var item in results)
            {
                Console.WriteLine(
                    $"\t{item.Id,-10}" +
                    $"{item.Name,-50}" +
                    $"{item.Quantity,-15}" +
                    $"{item.TotalValue,-20}"
                );
            }
        }

        private static async Task GetFullItemDetails()
        {
            if (_itemsService is null)
                return;

            Console.WriteLine("-> Items Details : ");

            var result = await _itemsService.GetItemsWithGenresAndCategories();
            
            Console.WriteLine($"\t{"ID",-5}{"ItemName",-40}{"PlayerName",-20}{"Category",-10}{"GenreName",-10}{"ItemDescription",-50}");

            foreach (var item in result)
            {
                Console.WriteLine(
                   $"\t{item.Id,-5}" +
                   $"{item.ItemName,-40}" +
                   $"{item.PlayerName,-20}" +
                   $"{item.Category,-10}" +
                   $"{item.GenreName,-10}" +
                   $"{item.ItemDescription,-50}"
               );
            }
        }

        private static async Task ListCategoriesAndColors()
        {
            if (_categoriesService is null)
                return;

            Console.WriteLine("-> List categories and colors :");

            var results = await _categoriesService.ListCategoriesAndDetails();

            foreach (var c in results)
            {
                Console.WriteLine($"\tCategory [{c.Category}] is {c.CategoryDetails?.Color}");
            }

            _categories = results;
        }
        
        private static async Task CreateMultipleItems()
        {
            if (_itemsService is null)
                return;

            Console.Write("\tWould you like to create items as a batch? => ");
            bool batchCreate = Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false;
            var allItems = new List<CreateOrUpdateItemDTO>();

            bool createAnother = true;
            while(createAnother is true)
            {
                var newItem = new CreateOrUpdateItemDTO();
                Console.WriteLine("\t\tCreating a new item...");
                Console.Write("\t\tPlease enter the name : ");
                newItem.Name = Console.ReadLine();
                Console.Write("\t\tPlease enter the description : ");
                newItem.Description = Console.ReadLine();
                Console.Write("\t\tPlease enter the notes : ");
                newItem.Notes = Console.ReadLine();
                Console.Write("\t\tPlease enter the Category [B]ooks, [M]ovies, [G]ames : ");
                newItem.CategoryId = GetCategoryId(Console.ReadLine()?.Substring(0, 1).ToUpper());

                if(batchCreate is false)
                {
                    await _itemsService.UpsertItem(newItem);
                }
                else
                {
                    allItems.Add(newItem);
                }

                Console.Write($"\t\tWould you like to create another item? => ");
                createAnother = Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false;

                if (batchCreate is true && createAnother is false)
                    await _itemsService.UpsertItems(allItems);
            }
        }
        
        private static async Task UpdateMultipleItems()
        {
            if (_itemsService is null)
                return;

            Console.Write("\tWould you like to update items as a batch? => ");
            bool batchUpdate = Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false;
            var allItems = new List<CreateOrUpdateItemDTO>();

            bool updateAnother = true;
            while(updateAnother is true)
            {
                Console.WriteLine($"\t\tItems:");
                var items = await _itemsService.GetItems();
                Console.WriteLine($"\t\t{"ID", -5}{"Name", -50}");
                items.ForEach(x => Console.WriteLine($"\t\t{x.Id,-5}{x.Name,-50}"));
                Console.WriteLine($"\t\t*********************************************");
                Console.Write($"\t\tEnter the ID number to update : ");
                if (int.TryParse(Console.ReadLine(),  out int id))
                {
                    var itemMatch = items.FirstOrDefault(x => x.Id == id);
                    if(itemMatch is not null)
                    {
                        var updateItem = _mapper.Map<CreateOrUpdateItemDTO>(_mapper.Map<Item>(itemMatch));
                        Console.Write($"\t\tEnter the new name [Leave blank to keep existing] : ");
                        var newName = Console.ReadLine();
                        updateItem.Name = !string.IsNullOrWhiteSpace(newName) ? 
                                                    newName : updateItem.Name;
                        Console.Write($"\t\tEnter the new desc [Leave blank to keep existing] :");
                        var newDesc = Console.ReadLine();
                        updateItem.Description = !string.IsNullOrWhiteSpace(newDesc) ?
                                                    newDesc : updateItem.Description;
                        Console.Write($"\t\tEnter the new notes [Leave blank to keep exisiting] : ");
                        var newNotes = Console.ReadLine();
                        updateItem.Notes = !string.IsNullOrWhiteSpace(newNotes) ?
                                                    newNotes : updateItem.Notes;
                        Console.Write($"\t\tToggle Item Active Status? [Yes / No] : ");
                        var toggleActive = Console.ReadLine().Substring(0, 1).Equals("y", StringComparison.OrdinalIgnoreCase);
                        if (toggleActive)
                        {
                            updateItem.IsActive = !updateItem.IsActive;
                        }

                        Console.Write($"\t\tEnter the category - [B]ooks, [M]ovies, [G]ames, or [N]o change : ");
                        var userChoice = Console.ReadLine().Substring(0, 1).ToUpper();
                        updateItem.CategoryId = userChoice.Equals("N", StringComparison.OrdinalIgnoreCase) ? itemMatch.CategoryId :
                                    GetCategoryId(userChoice);

                        if(batchUpdate is false)
                        {
                            await _itemsService.UpsertItem(updateItem);
                        }
                        else
                        {
                            allItems.Add(updateItem);
                        }
                    }
                }

                Console.Write($"\tWould you like to update another? => ");
                updateAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);
                if (batchUpdate is true && updateAnother is false)
                {
                    await _itemsService.UpsertItems(allItems);
                }
            }
        }

        private static async Task DeleteMultipleItems()
        {
            if (_itemsService is null)
                return;

            Console.Write("\tWould you like to delete items as a batch? => ");
            bool batchDelete = Console.ReadLine()?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false;
            var allItems = new List<int>();

            bool deleteAnother = true;
            while(deleteAnother is true)
            {
                Console.WriteLine($"\t\tItems:");
                var items = await _itemsService.GetItems();
                Console.WriteLine($"\t\t{"ID", -5}{"Name", -50}");
                items.ForEach(x => Console.WriteLine($"\t\t{x.Id,-5}{x.Name,-50}"));
                Console.WriteLine($"\t\t*********************************************");
                
                if (batchDelete is true && allItems.Any())
                {
                    Console.WriteLine($"\t\tItems scheduled for delete");
                    allItems.ForEach(x => Console.Write($"{x},"));
                    Console.WriteLine($"");
                    Console.WriteLine($"*********************************************");
                }

                Console.Write($"\t\tEnter the ID number to delete : ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var itemMatch = items.FirstOrDefault(x => x.Id == id);
                    if(itemMatch is not null)
                    {
                        if (batchDelete)
                        {
                            if (!allItems.Contains(itemMatch.Id))
                            {
                                allItems.Add(itemMatch.Id);
                            }
                        }
                        else
                        {
                            Console.Write($"\t\tAre you sure you want to delete the item {itemMatch.Id} - {itemMatch.Name} [Yes/No] : ");
                            if (Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase))
                            {
                                await _itemsService.DeleteItem(itemMatch.Id);
                                Console.WriteLine($"\t\tItem Deleted.");
                            }
                        }
                    }
                }

                Console.Write($"\tWould you like to delete another? => ");
                deleteAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

                if(batchDelete is true && deleteAnother is false)
                {
                    Console.Write($"Are you sure you want to delete the following items: ");
                    allItems.ForEach(x => Console.Write($"{x},"));
                    Console.WriteLine($"");
                    if (Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase))
                    {
                        await _itemsService.DeleteItems(allItems);
                        Console.WriteLine($"Items Deleted.");
                    }
                }
            }
        }

        private static int GetCategoryId(string? name)
        {
            switch(name)
            {
                case "B":
                    return _categories?.FirstOrDefault(x => x.Category.ToLower().Equals("books"))?.Id ?? -1;
                
                case "M":
                return _categories?.FirstOrDefault(x => x.Category.ToLower().Equals("movies"))?.Id ?? -1;
                
                case "G":
                return _categories?.FirstOrDefault(x => x.Category.ToLower().Equals("games"))?.Id ?? -1;

                default: return -1;
            }
        }
        #endregion

    }
}