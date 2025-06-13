using AutoMapper;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.AutoMappers;

public class PaginationTankMapper : Profile
{
    public PaginationTankMapper()
    {
        CreateMap<PaginationResponse<TankEntity>, PaginationResponseDto<TankDto>>().ReverseMap();
    }
}
