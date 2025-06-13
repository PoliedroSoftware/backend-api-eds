using AutoMapper;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Application.Hose.AutoMappers
{
    public class PaginationHoseMapper : Profile
    {
        public PaginationHoseMapper()
        {
            CreateMap<PaginationResponse<HoseEntity>, PaginationResponseDto<HoseDto>>().ReverseMap();
        }
    }
}
