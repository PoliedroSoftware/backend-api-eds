using AutoMapper;
using Poliedro.Eds.Application.Inventory.Commands;
using Poliedro.Eds.Application.Inventory.Dtos;
using Poliedro.Eds.Application.Shopping.Commands.CreateShopping;
using Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.Shopping.AutoMappers;
public class ShoppingMapper : Profile
{
    public ShoppingMapper()
    {
        CreateMap<ShoppingEntity, Dtos.ShoppingDto>().ReverseMap();
        CreateMap<ShoppingEntity, CreateShoppingCommand>().ReverseMap();
        CreateMap<ShoppingEntity, CreateShoppingRequestDto>().ReverseMap();
        CreateMap<ShoppingEntity, UpdateShoppingCommand>().ReverseMap();

        CreateMap<ShoppingProductEntity, ShoppingProductDto>().ReverseMap();
        CreateMap<ShoppingProductEntity, ShoppingProductRequestDto>().ReverseMap();

        CreateMap<InventoryEntity, InventoryDto>().ReverseMap();
        CreateMap<InventoryEntity, InventoryCommand>().ReverseMap();
    }
}
