using AutoMapper;
using EFCore.Inventory.DatabaseAccessLayer;
using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Models.DTOs;

namespace EFCore.Inventory.BusinessLogicLayer
{
    public class CategoriesService : ICategoriesService
    {
        #region Private Members

        private readonly ICategoriesRepo _dbRepo;

        #endregion

        #region Constructors

        public CategoriesService(InventoryDbContext context, IMapper mapper)
        {
            _dbRepo = new CategoriesRepo(context, mapper);
        }

        #endregion

        #region Public Methods

        public List<CategoryDTO> ListCategoriesAndDetails()
        {
            return _dbRepo.ListCategoriesAndDetails();
        }

        #endregion
    }
}
