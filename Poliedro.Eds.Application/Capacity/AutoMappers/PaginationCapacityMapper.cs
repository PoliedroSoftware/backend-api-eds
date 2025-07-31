using AutoMapper;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Capacity.AutoMappers;

public class PaginationCapacityMapper : Profile
{
    public PaginationCapacityMapper()
    {
        CreateMap<PaginationResponse<CapacityEntity>, PaginationResponseDto<CapacityDto>>().ReverseMap();
    }
}
