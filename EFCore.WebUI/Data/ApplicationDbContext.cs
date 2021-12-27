using EFCore.InventoryModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.WebUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetails> CategoryDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}