using AutoMapper;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.AutoMappers;

    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<ProductEntity>, PaginationResponseDto<ProductDto>>().ReverseMap();
        }
    }

