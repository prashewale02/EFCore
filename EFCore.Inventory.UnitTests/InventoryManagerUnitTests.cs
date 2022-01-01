using EFCore.Inventory.BusinessLogicLayer;
using EFCore.Inventory.DatabaseAccessLayer;
using EFCore.Inventory.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace EFCore.Inventory.UnitTests
{
    [TestClass]
    public class InventoryManagerUnitTests
    {
        private IItemsService _itemsService;
        private Mock<IItemsRepo> _itemsRepo;
        
        [TestInitialize]
        public void InitializeTests()
        {
            //_itemsService = new ItemsService();
            _itemsRepo = new Mock<IItemsRepo>();
            var items = new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    Name = "Star Wars IV: A New Hope",
                    Description = "Luke's Friends",
                    CategoryId = 2
                },
                new Item()
                {
                    Id = 2,
                    Name = "Star Wars V: The Empire Strikes Back",
                    Description = "Luke's Dad",
                    CategoryId = 2
                },
                new Item()
                {
                    Id = 3,
                    Name = "Star Wars VI: The Return of the Jedi",
                    Description = "Luke's Sister",
                    CategoryId = 2
                }
            };

            _itemsRepo.Setup(m => m.GetItems()).Returns(items);
        }

        [TestMethod]
        public void Get_Items()
        {
            var result = _itemsService.GetItems();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}