using AutoMapper;
using EFCore.Inventory.BusinessLogicLayer;
using EFCore.Inventory.DatabaseAccessLayer;
using EFCore.Inventory.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.Inventory.UnitTests
{
    [TestClass]
    public class InventoryManagerUnitTests
    {
        private IItemsService _itemsService;
        private Mock<IItemsRepo> _itemsRepo;

        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;
        public TestContext TestContext { get; set; }
        

        [ClassInitialize]
        public static void InitializeTestEnvironment(TestContext testContext)
        {
            _mapper = BuildMapper();
        }

        private static IMapper BuildMapper()
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

            Console.WriteLine("\tAutomapper Profile successfully configured.");

            return _mapperConfig.CreateMapper();
        }

        [TestInitialize]
        public void InitializeTests()
        {
            InstatiateItemsRepoMock();
            _itemsService = new ItemsService(_itemsRepo.Object, _mapper);
        }

        private void InstatiateItemsRepoMock()
        {
            _itemsRepo = new Mock<IItemsRepo>();
            var items = GetItemsTestData();

            _itemsRepo.Setup(m => m.GetItems()).Returns(Task.FromResult(items));
        }

        private List<Item> GetItemsTestData()
        {
            return new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    Name = Constants.TITLE_NEWHOPE,
                    Description = Constants.DESC_NEWHOPE,
                    CategoryId = 2
                },
                new Item()
                {
                    Id = 2,
                    Name = Constants.TITLE_EMPIRE,
                    Description = Constants.DESC_EMPIRE,
                    CategoryId = 2
                },
                new Item()
                {
                    Id = 3,
                    Name = Constants.TITLE_RETRUN,
                    Description = Constants.DESC_RETURN,
                    CategoryId = 2
                }
            };
        }

        [TestMethod]
        public async Task Get_Items()
        {
            var result = await _itemsService.GetItems();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);
            var expected = GetItemsTestData();

            var item1 = result[0];
            item1.Name.ShouldBe(Constants.TITLE_NEWHOPE);
            item1.Description.ShouldBe(Constants.DESC_NEWHOPE);

            var item2 = result[1];
            item2.Name.ShouldBe(expected[1].Name);
            item2.Description.ShouldBe(expected[1].Description);

        }
    }
}