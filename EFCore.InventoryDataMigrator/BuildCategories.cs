using EFCore.DBLibrary;
using EFCore.InventoryModels;

namespace EFCore.InventoryDataMigrator
{
    internal class BuildCategories
    {
        private readonly InventoryDbContext _context;
        private const string SEED_USER_ID = "31412031-7859-429c-ab21-c2e3e8d98042";

        public BuildCategories(InventoryDbContext context)
        {
            _context = context;
        }

        public void ExecuteSeed()
        {
            // Seed Categories Here.
            if (_context.Categories.Count() == 0)
            {
                _context.Categories.AddRange
                (
                    new Category()
                    {
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Movies",
                        CategoryDetails = new CategoryDetails()
                        {
                            ColorName = "Blue",
                            ColorValue = "#0000FF"
                        }
                    },
                    new Category()
                    {
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Books",
                        CategoryDetails = new CategoryDetails()
                        {
                            ColorName = "Red",
                            ColorValue = "#FF0000"
                        }
                    },
                    new Category()
                    {
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Games",
                        CategoryDetails = new CategoryDetails()
                        {
                            ColorName = "Green",
                            ColorValue = "#008000"
                        }
                    }
                );
                _context.SaveChanges();
            }
        }
    }
}