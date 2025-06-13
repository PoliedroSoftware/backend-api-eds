using AutoMapper;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Application.ProductType.AutoMappers
{
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<ProductTypeEntity>, PaginationResponseDto<ProductTypeDto>>().ReverseMap();
        }
    }
}
