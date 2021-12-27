using EFCore.InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore.DBLibrary
{
    public class InventoryDbContext : DbContext
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;

        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";

        #endregion


        #region public Properties

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetails> CategoryDetails { get; set; }

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

        #region Overriden Methods

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                        .HasMany(x => x.Players)
                        .WithMany(p => p.Items)
                        .UsingEntity<Dictionary<string, object>>(
                            "ItemPlayers",
                            ip => ip.HasOne<Player>()
                                    .WithMany()
                                    .HasForeignKey("PlayerId")
                                    .HasConstraintName(
                                        "FK_ItemPlayer_Players_PlayerId")
                                    .OnDelete(DeleteBehavior.Cascade),
                            ip => ip.HasOne<Item>()
                                    .WithMany()
                                    .HasForeignKey("ItemId")
                                    .HasConstraintName(
                                        "FK_PlayerItem_Items_ItemId")
                                    .OnDelete(DeleteBehavior.Cascade)
                        );
        }

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            foreach (var entry in tracker.Entries())
            {
                if (entry.Entity is FullAuditModel)
                {
                    var referenceEntity = entry.Entity as FullAuditModel;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedDate = DateTime.Now;
                            if(string.IsNullOrWhiteSpace(
                                referenceEntity.CreatedByUserId))
                            {
                                referenceEntity.CreatedByUserId = 
                                    _systemUserId;
                            }
                            break;

                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referenceEntity.LastModifiedDate = DateTime.Now;
                            if(string.IsNullOrWhiteSpace(
                                referenceEntity.LastModifiedUserId))
                            {
                                referenceEntity.LastModifiedUserId = 
                                    _systemUserId;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
        #endregion
    }
}
