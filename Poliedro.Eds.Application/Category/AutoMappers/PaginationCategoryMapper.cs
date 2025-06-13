using AutoMapper;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Category.AutoMappers;
    public class PaginationCategoryMapper : Profile
    {
        public PaginationCategoryMapper()
        {
            CreateMap<PaginationResponse<CategoryEntity>, PaginationResponseCategoryDto<CategoryDto>>().ReverseMap();
        }
    }

