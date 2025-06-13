using AutoMapper;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.AutoMappers;
    public class PaginationShoppingMapper : Profile
    {
        public PaginationShoppingMapper()
        {
            CreateMap<PaginationResponse<ShoppingEntity>, PaginationResponseShoppingDto<ShoppingDto>>().ReverseMap();
        }
    }

