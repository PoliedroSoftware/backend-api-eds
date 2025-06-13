using AutoMapper;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.AutoMappers
{
    public class PaginationDispenserTypeMapper : Profile
    {
        public PaginationDispenserTypeMapper()
        {
            CreateMap<PaginationResponse<DispenserTypeEntity>, PaginationResponseDispenserTypeDto<DispenserTypeDto>>().ReverseMap();
        }
    }
}
