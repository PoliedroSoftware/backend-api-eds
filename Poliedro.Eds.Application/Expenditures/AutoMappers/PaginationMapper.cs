using AutoMapper;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Application.Expenditures.AutoMappers;
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<ExpendituresEntity>, PaginationResponseDto<ExpendituresDto>>().ReverseMap();
        }
    }

