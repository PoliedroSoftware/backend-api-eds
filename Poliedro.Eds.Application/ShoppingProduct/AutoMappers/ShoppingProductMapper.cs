using AutoMapper;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.AutoMappers;
public class ShoppingProductMapper : Profile
{
    public ShoppingProductMapper()
    {
        CreateMap<ShoppingProductEntity, ShoppingProductDto>().ReverseMap();
        CreateMap<ShoppingProductEntity, CreateShoppingProductCommand>().ReverseMap();
        CreateMap<ShoppingProductEntity, CreateShoppingProductRequestDto>().ReverseMap();
        CreateMap<ShoppingProductEntity, UpdateShoppingProductCommand>().ReverseMap();
    }
}
