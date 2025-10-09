using AutoMapper;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.AutoMappers
{
    public class PaginationDispenserMapper : Profile
    {
        public PaginationDispenserMapper()
        {
            CreateMap<PaginationResponse<DispensersEntity>, PaginationResponseDto<DispensersDto>>().ReverseMap();
        }
    }
}
