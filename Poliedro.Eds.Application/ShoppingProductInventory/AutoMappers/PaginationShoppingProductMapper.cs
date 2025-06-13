using AutoMapper;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;

namespace Poliedro.Eds.Application.ShoppingProductInventory.AutoMappers
{
    public class PaginationShoppingProductInventoryMapper : Profile
    {
        public PaginationShoppingProductInventoryMapper()
        {
            CreateMap<PaginationResponse<ShoppingProductInventoryEntity>, PaginationResponseShoppingProductInventoryDto<ShoppingProductInventoryDto>>().ReverseMap();
        }
    }
}
