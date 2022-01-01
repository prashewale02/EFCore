using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Inventory.DbLibrary;
using EFCore.Inventory.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Inventory.DatabaseAccessLayer
{
    public class CategoriesRepo : ICategoriesRepo
    {
        #region Private Members

        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        #endregion

        #region Constructors

        public CategoriesRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        public List<CategoryDTO> ListCategoriesAndDetails()
        {
            return _context.Categories
                        .Include(x => x.CategoryDetails)
                        .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                        .ToList();
        } 

        #endregion
    }
}
