using EFCore.DBLibrary;
using EFCore.InventoryModels;

namespace EFCore.InventoryDataMigrator
{
    internal class BuildItems
    {
        #region Private Members

        private readonly InventoryDbContext _context;
        private const string SEED_USER_ID = "31412031-7859-429c-ab21-c2e3e8d98042";
        
        #endregion

        /// <summary>
        /// Constructor with <see cref="InventoryDbContext"/>
        /// </summary>
        /// <param name="context"></param>
        public BuildItems(InventoryDbContext context)
        {
            _context = context;
        }

        public void ExecuteSeed()
        {
            if (_context.Items.Count() == 0)
            {
                _context.Items.AddRange
                (
                    new Item()
                    {
                        Name = "Batman Begins",
                        CurrentOrFinalPrice = 29.99m,
                        Description = "You either die the hero or live long enough to see yourself become the villain",
                        IsOnSale = false,
                        Notes = "",
                        PurchasePrice = 23.99m,
                        PurchaseDate = null,
                        Quantity = 1000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now,
                                CreatedByUserId = SEED_USER_ID,
                                IsActive = true,
                                IsDeleted = false,
                                Description = "https://www.imdb.com/name/nm0000288/",
                                Name = "Christian Bale"
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Inception",
                        CurrentOrFinalPrice = 7.99m,
                        Description = "You mustn't afraid to dream a little bigger, darling",
                        IsOnSale = false,
                        Notes = "",
                        PurchasePrice = 4.99m,
                        PurchaseDate = null,
                        Quantity = 1000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now,
                                CreatedByUserId = SEED_USER_ID,
                                IsActive = true,
                                IsDeleted = false,
                                Description = "https://www.imdb.com/name/nm0000138/",
                                Name = "Leonardo DiCaprio"
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Remember the Titans",
                        CurrentOrFinalPrice = 3.99m,
                        Description = "Left Side, Strong Side",
                        IsOnSale = false,
                        Notes = "",
                        PurchasePrice = 7.99m,
                        PurchaseDate = null,
                        Quantity = 1000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now,
                                CreatedByUserId = SEED_USER_ID,
                                IsActive = true,
                                IsDeleted = false,
                                Description = "https://www.imdb.com/name/nm0000243/",
                                Name = "Denzel Washington"
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Star Wars: The Empire Strikes Back",
                        CurrentOrFinalPrice = 19.99m,
                        Description = "He will join us or die, master",
                        IsOnSale = false,
                        Notes = "",
                        PurchasePrice = 35.99m,
                        PurchaseDate = null,
                        Quantity = 1000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                             new Player()
                             {
                                 CreatedDate = DateTime.Now,
                                 IsActive = true,
                                 IsDeleted = false,
                                 CreatedByUserId = SEED_USER_ID,
                                 Description = "https://www.imdb.com/name/nm0000434/",
                                 Name = "Mark Hamill"
                             }
                        }
                    },
                    new Item()
                    {
                        Name = "Top Gun",
                        CurrentOrFinalPrice = 6.99m,
                        Description = "I feel the need, the need for speed!",
                        IsOnSale = false,
                        Notes = "",
                        PurchasePrice = 8.99m,
                        PurchaseDate = null,
                        Quantity = 1000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new
                        List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                IsDeleted = false,
                                CreatedByUserId = SEED_USER_ID,
                                Description = "https://www.imdb.com/name/nm0000129/",
                                Name = "Tom Cruise"
                            }
                        }
                    }
                );

                _context.SaveChanges();
            }
        }
    }
}
