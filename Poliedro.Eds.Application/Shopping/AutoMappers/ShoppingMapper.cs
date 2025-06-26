using AutoMapper;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.Shopping.AutoMappers;
public class ShoppingMapper : Profile
{
    public ShoppingMapper()
    {
        CreateMap<ShoppingEntity, ShoppingDto>().ReverseMap();
        CreateMap<ShoppingEntity, CreateShoppingCommand>().ReverseMap();
        CreateMap<ShoppingEntity, CreateShoppingRequestDto>().ReverseMap();
        CreateMap<ShoppingEntity, UpdateShoppingCommand>().ReverseMap();

        CreateMap<ShoppingProductEntity, ShoppingProductDto>().ReverseMap();
        CreateMap<ShoppingProductEntity, ShoppingProductRequestDto>().ReverseMap();
    }
}
