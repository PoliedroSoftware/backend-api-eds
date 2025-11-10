using AutoMapper;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.AutoMappers;

public class ShoppingProductMapper : Profile
{
    public ShoppingProductMapper()
    {
        CreateMap<ShoppingProductEntity, ShoppingProductDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ReverseMap();
        CreateMap<ShoppingProductEntity, CreateShoppingProductCommand>().ReverseMap();
        CreateMap<ShoppingProductEntity, CreateShoppingProductRequestDto>().ReverseMap();
        CreateMap<ShoppingProductEntity, UpdateShoppingProductCommand>().ReverseMap();
        
        CreateMap<ProductEntity, ProductDto>().ReverseMap();
    }
}
