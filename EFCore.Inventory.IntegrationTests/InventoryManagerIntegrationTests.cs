using AutoMapper;
using EFCore.Inventory.DatabaseAccessLayer;
using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFCore.Inventory.IntegrationTests
{
    public class InventoryManagerIntegrationTests
    {
        private DbContextOptions<InventoryDbContext> _options;

        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;

        private IItemsRepo _dbRepo;

        public InventoryManagerIntegrationTests()
        {
            SetupOptions();

            BuildDefaults();
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<InventoryDbContext>()
                            .UseInMemoryDatabase(databaseName: "InventoryManagerTest")
                            .Options;

            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(InventoryMapper));
            _serviceProvider = services.BuildServiceProvider();

            _mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<InventoryMapper>();
            });
            _mapperConfig.AssertConfigurationIsValid();
            _mapper = _mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Get_Items()
        {
            using (var context = new InventoryDbContext(_options))
            {
                // Act
                _dbRepo = new ItemsRepo(context, _mapper);
                var items = await _dbRepo.GetItems();

                // Assert
                items.ShouldNotBeNull();
                items.Count.ShouldBe(3);
                
                var first = items.First();
                first.Name.ShouldBe(Constants.ITEM1_NAME);
                first.Description.ShouldBe(Constants.ITEM1_DESC);
                first.Notes.ShouldBe(Constants.ITEM1_NOTES);
                first.Category.Name.ShouldBe(Constants.CAT1_NAME);

                var second = items.SingleOrDefault(x => x.Name.ToLower() == Constants.ITEM2_NAME.ToLower());
                second.ShouldNotBeNull();
                second.Description.ShouldBe(Constants.ITEM2_DESC);
                second.Notes.ShouldBe(Constants.ITEM2_NOTES);
                second.Category.Name.ShouldBe(Constants.CAT2_NAME);
            }
        }

        private void BuildDefaults()
        {
            using (var context = new InventoryDbContext(_options))
            {
                var item1Detail = context.Items.SingleOrDefault(x =>
                                        x.Name.Equals(Constants.ITEM1_NAME));
                var item2Detail = context.Items.SingleOrDefault(x =>
                                        x.Name.Equals(Constants.ITEM2_NAME));
                var item3Detail = context.Items.SingleOrDefault(x =>
                                        x.Name.Equals(Constants.ITEM3_NAME));

                if (item1Detail is not null && item2Detail is not null && item3Detail is not null)
                    return;

                var color1 = new CategoryDetails()
                {
                    ColorName = Constants.COLOR_BLUE,
                    ColorValue = Constants.COLOR_BLUE_VALUE
                };

                var color2 = new CategoryDetails()
                {
                    ColorName = Constants.COLOR_RED,
                    ColorValue = Constants.COLOR_RED_VALUE
                };

                var color3 = new CategoryDetails()
                {
                    ColorName = Constants.COLOR_GREEN,
                    ColorValue = Constants.COLOR_GREEN_VALUE
                };

                var cat1 = new Category()
                {
                    CategoryDetails = color1,
                    IsActive = true,
                    IsDeleted = false,
                    Name = Constants.CAT1_NAME
                };
                
                var cat2 = new Category()
                {
                    CategoryDetails = color2,
                    IsActive = true,
                    IsDeleted = false,
                    Name = Constants.CAT2_NAME
                };
                
                var cat3 = new Category()
                {
                    CategoryDetails = color3,
                    IsActive = true,
                    IsDeleted = false,
                    Name = Constants.CAT3_NAME
                };

                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);
                context.SaveChanges();

                var category1 = context.Categories.Single(x => 
                                    x.Name.Equals(Constants.CAT1_NAME));
                var category2 = context.Categories.Single(x => 
                                    x.Name.Equals(Constants.CAT2_NAME));
                var category3 = context.Categories.Single(x => 
                                    x.Name.Equals(Constants.CAT3_NAME));

                var item1 = new Item()
                {
                    Name = Constants.ITEM1_NAME,
                    Description = Constants.ITEM1_DESC,
                    Notes = Constants.ITEM1_NOTES,
                    IsActive = true,
                    IsDeleted = false,
                    CategoryId = category1.Id
                };
                context.Items.Add(item1);

                var item2 = new Item()
                {
                    Name = Constants.ITEM2_NAME,
                    Description = Constants.ITEM2_DESC,
                    Notes = Constants.ITEM2_NOTES,
                    IsActive = true,
                    IsDeleted = false,
                    CategoryId = category2.Id
                };
                context.Items.Add(item2);
                
                var item3 = new Item()
                {
                    Name = Constants.ITEM3_NAME,
                    Description = Constants.ITEM3_DESC,
                    Notes = Constants.ITEM3_NOTES,
                    IsActive = true,
                    IsDeleted = false,
                    CategoryId = category3.Id
                };
                context.Items.Add(item3);
                context.SaveChanges();

            }
        }

        [Theory]
        [InlineData(Constants.CAT1_NAME, Constants.COLOR_BLUE, Constants.COLOR_BLUE_VALUE)]
        [InlineData(Constants.CAT2_NAME, Constants.COLOR_RED, Constants.COLOR_RED_VALUE)]
        [InlineData(Constants.CAT3_NAME, Constants.COLOR_GREEN, Constants.COLOR_GREEN_VALUE)]
        public async Task Category_Has_Colors(string name, string color, string colorValue)
        {
            using (var context = new InventoryDbContext(_options))
            {
                // Act 
                var categoriesRepo = new CategoriesRepo(context, _mapper);
                var categories = await categoriesRepo.ListCategoriesAndDetails();

                // Assert
                categories.ShouldNotBeNull();
                categories.Count.ShouldBe(3);

                var category = categories.FirstOrDefault(x => x.Category.Equals(name));
                category.ShouldNotBeNull();
                category.CategoryDetails.Color.ShouldBe(color);
                category.CategoryDetails.Value.ShouldBe(colorValue);
            }
        }
    }
}