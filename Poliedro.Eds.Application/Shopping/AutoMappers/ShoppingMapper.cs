using AutoMapper;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Application.Inventory.Commands;
using Poliedro.Eds.Application.Inventory.Dtos;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.Shopping.AutoMappers;

public class ShoppingMapper : Profile
{
    public ShoppingMapper()
    {
        CreateMap<ShoppingEntity, Dtos.ShoppingDto>()
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ReverseMap();
            
        CreateMap<ShoppingEntity, CreateShoppingCommand>().ReverseMap();
        CreateMap<ShoppingEntity, CreateShoppingRequestDto>().ReverseMap();
        CreateMap<ShoppingEntity, UpdateShoppingCommand>().ReverseMap();

        CreateMap<ShoppingProductEntity, ShoppingProductDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ReverseMap();
        CreateMap<ShoppingProductEntity, ShoppingProductRequestDto>().ReverseMap();

        CreateMap<ProviderEntity, ProviderDto>().ReverseMap();
        CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
        CreateMap<ProductEntity, ProductDto>().ReverseMap();

        CreateMap<InventoryEntity, InventoryDto>().ReverseMap();
        CreateMap<InventoryEntity, InventoryCommand>().ReverseMap();
    }
}
