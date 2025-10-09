using AutoMapper;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.AutoMappers
{
    public class PaginationHoseHistoryMapper : Profile
    {
        public PaginationHoseHistoryMapper()
        {
            CreateMap<PaginationResponse<HoseHistoryEntity>, PaginationResponseDto<HoseHistoryDto>>().ReverseMap();
        }
    }
}
