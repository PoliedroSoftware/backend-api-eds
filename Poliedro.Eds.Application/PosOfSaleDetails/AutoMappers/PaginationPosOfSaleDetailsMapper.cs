using AutoMapper;
using Poliedro.Eds.Application.PosOfSale.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.PosOfSaleDetails.Dtos;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.AutoMappers
{
    public class PaginationPosOfSaleDetailsMapper : Profile
    {
        public PaginationPosOfSaleDetailsMapper()
        {
            CreateMap<PaginationResponse<PosOfSaleDetailsEntity>, PaginationResponseDto<PosOfSaleDetailsDto>>().ReverseMap();
        }
    }
}
