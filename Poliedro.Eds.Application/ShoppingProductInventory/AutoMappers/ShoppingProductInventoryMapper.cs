using AutoMapper;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Commands.UpdateShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.AutoMappers
{
    public class ShoppingProductInventoryMapper : Profile
    {
        public ShoppingProductInventoryMapper()
        {


            CreateMap<ShoppingProductInventoryEntity, ShoppingProductInventoryDto>().ReverseMap();
            CreateMap<ShoppingProductInventoryEntity, CreateShoppingProductInventoryCommand>().ReverseMap();
            CreateMap<ShoppingProductInventoryEntity, CreateShoppingProductInventoryRequestDto>().ReverseMap();
            CreateMap<ShoppingProductInventoryEntity, UpdateShoppingProductInventoryCommand>().ReverseMap();

        }
    }

}
