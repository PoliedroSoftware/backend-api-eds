using AutoMapper;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Application.Capacity.AutoMappers;

public class PaginationCapacityMapper : Profile
{
    public PaginationCapacityMapper()
    {
        CreateMap<PaginationResponse<CapacityEntity>, PaginationResponseDto<CapacityDto>>().ReverseMap();
    }
}