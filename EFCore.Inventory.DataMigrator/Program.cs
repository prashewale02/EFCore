using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore.Inventory.DataMigrator 
{
    class Program
    {
        private static IConfigurationRoot? _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        private static void BuildOptions()
        {
            Console.WriteLine("-> Configuring connection to the database.");

            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(
                _configuration.GetConnectionString("InventoryManager"));

            Console.WriteLine("\tSuccessfully connected to the databaase.");
        }

        private static void ApplyMigrations()
        {
            Console.WriteLine("-> Applying Migration.");
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                db.Database.Migrate();
            }
            Console.WriteLine("\tMigration applied successfully.");
        }

        private static void ExecuteCustomSeedData()
        {
            Console.WriteLine("-> Excuting Seed Data.");
            using (var context = new InventoryDbContext(_optionsBuilder.Options))
            {
                Console.WriteLine("\tSeeding Categories...");
                var categories = new BuildCategories(context);
                categories.ExecuteSeed();
                Console.WriteLine("\tCategories data seeded successfully.");
                
                Console.WriteLine();

                Console.WriteLine("\tSeeding Items...");
                var items = new BuildItems(context);
                items.ExecuteSeed();
                Console.WriteLine("\tItems data seeded successfully.");
            }

            Console.WriteLine("\tSeeding data executed successfully.");
        }

        static void Main(String[] args)
        {
            Console.WriteLine("******* Welcome to Inventory Data Migrator *******");

            BuildOptions();

            ExecuteCustomSeedData();
            
            ApplyMigrations();

            Console.ReadLine();
        }
    }
}
