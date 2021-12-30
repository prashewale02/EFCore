using AutoMapper;
using EFCore.InventoryModels;
using EFCore.InventoryModels.DTOs;

namespace EFCore.ConsoleApp.InventoryManager
{
    internal class InventoryMapper : Profile
    {
        public InventoryMapper()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<Category, CategoryDTO>()
                .ForMember(x => x.Category, opt => opt.MapFrom(y => y.Name))
                .ReverseMap()
                .ForMember(y => y.Name, opt => opt.MapFrom(x => x.Category));

            CreateMap<CategoryDetails, CategoryDetailDTO>()
                .ForMember(x => x.Color, opt => opt.MapFrom(y => y.ColorName))
                .ForMember(x => x.Value, opt => opt.MapFrom(y => y.ColorValue))
                .ReverseMap()
                .ForMember(y => y.ColorName, opt => opt.MapFrom(x => x.Color))
                .ForMember(y => y.ColorValue, opt => opt.MapFrom(x => x.Value));

        }
    }
}
