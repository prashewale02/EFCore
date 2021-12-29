using EFCore.DBLibrary.InventoryManager;
using EFCore.InventoryModels;
using EFCore.InventoryHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace EFCore.ConsoleApp.InventoryManager
{
    class Program
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";
        private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";

        #endregion

        #region Helper Methods

        static void BuildOptions()
        {
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

        private static void ListInventory()
        {
            Console.WriteLine("-> Inventory Items : ");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items = db.Items.OrderBy(x => x.Name).ToList();
                items.ForEach(x => Console.WriteLine($"\tNew Item: {x.Name}"));
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
                    var output = $"\tITEM {item.Name} - {item.Description}";
                    if (!string.IsNullOrWhiteSpace(item.CategoryName))
                    {
                        output = $"{output} has category : {item.CategoryName}";
                    }
                    Console.WriteLine(output);
                }
            }
        }

        private static void GetAllActiveItemsAsPipeDelimitedString()
        {
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var isActiveParam = new SqlParameter("IsActive", 1);
                var result = db.AllItemsOutput.FromSqlRaw(
                    @"SELECT [dbo].[ItemNamesPipeDelimitedString] (@IsActive)" +
                    "AllItems", isActiveParam).FirstOrDefault();

                Console.WriteLine($"-> All Active Items: {result.AllItems}");
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
                var result = db.FullItemDetails.FromSqlRaw(
                    "SELECT * FROM [dbo].[ViewFullItemDetails]" +
                    "ORDER BY ItemName, GenreName, Category, PlayerName"
                    ).ToList();

                Console.WriteLine($"\t{"ID",-10}{"ItemName",-50}{"PlayerName",-20}{"Category", -10}{"GenreName",-20}{"ItemDescription",-50}");

                foreach (var item in result)
                {
                    Console.WriteLine(
                       $"\t{item.Id,-10}" +
                       $"{item.ItemName,-50}" +
                       $"{item.PlayerName,-20}" +
                       $"{item.Category, -20}" +
                       $"{item.GenreName, -20}" +
                       $"{item.ItemDescription,-50}"
                   );
                }
            }
        }

        #endregion

        static void Main(String[] args)
        {
            Console.WriteLine("***** Welcome to Entity Framework Core 6 *****");

            BuildOptions();
            Console.WriteLine();

            ListInventory();
            Console.WriteLine();

            GetItemsForListing();
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

    }
}