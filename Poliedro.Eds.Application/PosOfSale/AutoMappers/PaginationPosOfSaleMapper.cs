using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.PosOfSale.Dtos;
using Poliedro.Eds.Application.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.AutoMappers
{
    public class PaginationPosOfSaleMapper : Profile
    {
        public PaginationPosOfSaleMapper()
        {
            CreateMap<PaginationResponse<HoseEntity>, PaginationResponseDto<PosOfSaleDto>>().ReverseMap();
        }
    }
}
