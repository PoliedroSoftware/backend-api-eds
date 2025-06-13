using AutoMapper;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Application.CompartimentCapacity.AutoMappers;
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<CompartimentCapacityEntity>, PaginationResponseDto<CompartimentCapacityDto>>().ReverseMap();
        }
    }

