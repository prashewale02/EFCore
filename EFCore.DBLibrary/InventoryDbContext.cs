using EFCore.InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore.DBLibrary
{
    public class InventoryDbContext : DbContext
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;

        #endregion


        #region public Properties

        public DbSet<Item> Items { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for scaffolding
        /// </summary>
        public InventoryDbContext() { }

        /// <summary>
        /// The Complex constructor for allowing dependency injection.
        /// </summary>
        /// <param name="options"></param>
        public InventoryDbContext(DbContextOptions options)
            : base(options) 
        {
            // Intentionally Empty
        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json",
                                    optional: true, reloadOnChange: true);

                _configuration = builder.Build();
                var connectionString =
                    _configuration.GetConnectionString("InventoryManager");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
