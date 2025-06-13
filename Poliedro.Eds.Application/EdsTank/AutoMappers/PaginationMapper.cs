using AutoMapper;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.AutoMappers;

    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<EdsTankEntity>, PaginationResponseDto<EdsTankDto>>().ReverseMap();
        }
    }

