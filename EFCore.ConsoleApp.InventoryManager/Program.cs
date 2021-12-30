using EFCore.DBLibrary.InventoryManager;
using EFCore.InventoryHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using EFCore.InventoryModels.DTOs;
using EFCore.InventoryModels;
using AutoMapper.QueryableExtensions;

namespace EFCore.ConsoleApp.InventoryManager
{
    class Program
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";
        private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";

        private static MapperConfiguration? _mapperConfig;
        private static IMapper? _mapper;
        private static IServiceProvider? _serviceProvider;

        #endregion

        static void Main(String[] args)
        {
            Console.WriteLine("***** Welcome to Entity Framework Core 6 *****");

            BuildOptions();

            BuildMapper();

            ListInventory();
            Console.WriteLine();

            ListInventoryWithProjection();
            Console.WriteLine();

            ListCategoriesAndColors();
            Console.WriteLine();

            GetItemsForListing();
            Console.WriteLine();

            GetItemsForListingLinq();
            Console.WriteLine();

            GetAllActiveItemsAsPipeDelimitedString();
            Console.WriteLine();

            GetItemsTotalValues();
            Console.WriteLine();

            GetFullItemDetails();
            Console.WriteLine();

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

        private static void BuildMapper()
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
            _mapper = _mapperConfig.CreateMapper();

            Console.WriteLine("\tAutomapper Profile successfully configured.");
        }

        private static void ListInventory()
        {
            Console.WriteLine("-> Inventory Items : ");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items = db.Items.OrderBy(x => x.Name).Take(20)
                                    .Select(x => new ItemDTO
                                    {
                                        Name = x.Name,
                                        Description = x.Description
                                    })
                                    .ToList();
                
                //var result = _mapper.Map<List<Item>, List<ItemDTO>>(items);
                items.ForEach(x => Console.WriteLine($"\t{x}"));
            }
        }

        private static void GetItemsForListing()
        {
            Console.WriteLine("-> Getting Items For Listing :");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var results = db.ItemsForListing.FromSqlRaw(
                    "EXECUTE dbo.GetItemsForListing").ToList();

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
        }

        private static void GetItemsForListingLinq() 
        {
            Console.WriteLine("-> Getting Items for listing using Linq.");
            var minDateValue = new DateTime(2021, 1, 1);
            var maxDateValue = new DateTime(2024, 1, 1);
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var results = db.Items.Include(x => x.Category)
                                      .ToList()
                                      .Select(x => new ItemDTO
                {
                    CreatedDate = x.CreatedDate,
                    CategoryName = x.Category.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    Notes = x.Notes,
                    CategoryId = x.CategoryId,
                    Id = x.Id
                }).Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
                  .OrderBy(y => y.CategoryName).ThenBy(z => z.Name)
                  .ToList();
                foreach (var itemDTO in results)
                {
                    Console.WriteLine($"\t{itemDTO}");
                }
            }
        }

        private static void GetAllActiveItemsAsPipeDelimitedString()
        {
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                //var isActiveParam = new SqlParameter("IsActive", 1);
                //var result = db.AllItemsOutput.FromSqlRaw(
                //    @"SELECT [dbo].[ItemNamesPipeDelimitedString] (@IsActive)" +
                //    "AllItems", isActiveParam).FirstOrDefault();

                var result = db.Items.Where(x => x.IsActive).ToList();
                var pipeDelimitedString = string.Join("|", result);

                Console.WriteLine($"-> All Active Items: {pipeDelimitedString ?? "No Items Found."}");
            }
        }

        private static void GetItemsTotalValues()
        {
            Console.WriteLine("-> Items Totals : ");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var isActiveParam = new SqlParameter("IsActive", 1);

                var result = db.GetItemsTotalValues.FromSqlRaw(
                        "SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", 
                        isActiveParam
                    ).ToList();

                Console.WriteLine($"\t{"ID", -10}{"Name", -50}{"Quantity", -15}{"TotalValue",-20}");

                foreach(var item in result)
                {
                    Console.WriteLine(
                        $"\t{item.Id, -10}" +
                        $"{item.Name, -50}" +
                        $"{item.Quantity, -15}" +
                        $"{item.TotalValue, -20}"
                    );
                }
            }
        }

        private static void GetFullItemDetails()
        {
            Console.WriteLine("-> Items Details : ");

            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                //var result = db.FullItemDetails.FromSqlRaw(
                //    "SELECT * FROM [dbo].[ViewFullItemDetails]" +
                //    "ORDER BY ItemName, GenreName, Category, PlayerName"
                //    ).ToList();

                var result = db.FullItemDetails.FromSqlRaw("SELECT * FROM [dbo].[ViewFullItemDetails]")
                                               .ToList()
                                               .OrderBy(x => x.ItemName)
                                               .ThenBy(x => x.GenreName)
                                               .ThenBy(x => x.Category)
                                               .ThenBy(x => x.PlayerName);

                Console.WriteLine($"\t{"ID",-5}{"ItemName",-30}{"PlayerName",-20}{"Category", -10}{"GenreName",-10}{"ItemDescription",-50}");

                foreach (var item in result)
                {
                    Console.WriteLine(
                       $"\t{item.Id,-5}" +
                       $"{item.ItemName,-30}" +
                       $"{item.PlayerName,-20}" +
                       $"{item.Category, -10}" +
                       $"{item.GenreName, -10}" +
                       $"{item.ItemDescription,-50}"
                   );
                }
            }
        }

        private static void ListInventoryWithProjection()
        {
            Console.WriteLine("-> List inventory with projection: ");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items =
                    db.Items
                            .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
                            .ToList();
                items.OrderBy(x => x.Name).ToList().ForEach(x => Console.WriteLine($"\t{x}"));

                //items.ForEach(x => Console.WriteLine($"\t{x}"));
            }
        }

        private static void ListCategoriesAndColors()
        {
            Console.WriteLine("-> List categories and colors :");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var results = db.Categories
                    .Include(x => x.CategoryDetails)
                    .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                    .ToList();

                foreach (var c in results)
                {
                    Console.WriteLine($"\tCategory [{c.Category}] is {c.CategoryDetails?.Color}");
                }
            }
        }

        #endregion

    }
}