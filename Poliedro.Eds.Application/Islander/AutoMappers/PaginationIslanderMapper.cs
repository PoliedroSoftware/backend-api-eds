using AutoMapper;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Application.Islander.AutoMappers
{
    public class PaginationIslanderMapper : Profile
    {
        public PaginationIslanderMapper()
        {
            CreateMap<PaginationResponse<IslanderEntity>, PaginationResponseDto<IslanderDto>>().ReverseMap();
        }
    }
}
