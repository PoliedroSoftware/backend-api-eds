using AutoMapper;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.AutoMappers;

public class PaginationEdsMapper : Profile
{
    public PaginationEdsMapper()
    {
        CreateMap<PaginationResponse<EdsEntity>, PaginationResponseDto<EdsDto>>().ReverseMap();
    }
}