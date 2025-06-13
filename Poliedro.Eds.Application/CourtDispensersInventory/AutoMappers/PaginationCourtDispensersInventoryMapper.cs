using AutoMapper;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.AutoMappers
{
    public class PaginationCourtDispensersInventoryMapper : Profile
    {
        public PaginationCourtDispensersInventoryMapper()
        {
            CreateMap<PaginationResponse<CourtDispensersInventoryEntity>, PaginationResponseDto<CourtDispensersInventoryDto>>().ReverseMap();
        }
    }
}
