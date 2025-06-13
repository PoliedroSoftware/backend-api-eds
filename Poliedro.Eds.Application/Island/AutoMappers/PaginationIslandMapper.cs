using AutoMapper;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Application.Island.AutoMappers
{
    public class PaginationIslandMapper : Profile
    {
        public PaginationIslandMapper()
        {
            CreateMap<PaginationResponse<IslandEntity>, PaginationResponseDto<IslandDto>>().ReverseMap();
        }
    }
}
