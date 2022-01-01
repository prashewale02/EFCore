using EFCore.Inventory.Models;
using EFCore.Inventory.Models.DTOs;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore.Inventory.DbLibrary
{
    public class InventoryDbContext : DbContext
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;

        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";

        #endregion

        #region public Properties

        #region Tables

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetails> CategoryDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }

        #endregion

        #region Procedures

        public DbSet<GetItemsForListingDTO> ItemsForListing { get; set; }

        #endregion

        #region Functions

        public DbSet<AllItemsPipeDelimitedStringDTO> AllItemsOutput { get; set; }
        public DbSet<GetItemsTotalValueDTO> GetItemsTotalValues { get; set; }
        #endregion

        #region Views

        public DbSet<FullItemDetailsDTO> FullItemDetails { get; set; }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for scaffolding
        /// </summary>
        public InventoryDbContext() 
        {

        }

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

            modelBuilder.Entity<GetItemsForListingDTO>(x =>
            {
                x.HasNoKey();
                x.ToView("ItemsForListing");
            });

            modelBuilder.Entity<AllItemsPipeDelimitedStringDTO>(x =>
            {
                x.HasNoKey();
                x.ToView("AllItemsOutput");
            });

            modelBuilder.Entity<GetItemsTotalValueDTO>(x =>
            {
                x.HasNoKey();
                x.ToView("GetItemsTotalValues");
            });

            modelBuilder.Entity<FullItemDetailsDTO>(x =>
            {
                x.HasNoKey();
                x.ToView("FullItemDetails");
            });

            // Seed default data to the database
            var genreCreateDate = new DateTime(2021, 01, 01);
            modelBuilder.Entity<Genre>(x =>
            {
                x.HasData 
                (
                    new Genre()
                    {
                        Id = 1,
                        CreatedDate = genreCreateDate,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Fantasy",
                        CreatedByUserId = _systemUserId
                    },
                    new Genre()
                    {
                        Id = 2,
                        CreatedDate = genreCreateDate,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Sci/Fi",
                        CreatedByUserId = _systemUserId
                    },
                    new Genre()
                    {
                        Id = 3,
                        CreatedDate = genreCreateDate,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Horror",
                        CreatedByUserId = _systemUserId
                    },
                    new Genre()
                    {
                        Id = 4,
                        CreatedDate = genreCreateDate,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Comedy",
                        CreatedByUserId = _systemUserId
                    },
                    new Genre()
                    {
                        Id = 5,
                        CreatedDate = genreCreateDate,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Drama",
                        CreatedByUserId = _systemUserId,
                    }
                );
            });
        }

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            foreach (var entry in tracker.Entries())
            {
                if (entry.Entity is FullAuditModel referenceEntity)
                {
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
