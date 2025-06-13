using AutoMapper;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Application.ShoppingProduct.AutoMappers;
    public class PaginationShoppingProductMapper : Profile
    {
        public PaginationShoppingProductMapper()
        {
            CreateMap<PaginationResponse<ShoppingProductEntity>, PaginationResponseShoppingProductDto<ShoppingProductDto>>().ReverseMap();
        }
    }

