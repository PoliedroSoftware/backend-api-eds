using AutoMapper;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.AutoMappers
{
    public class PaginationCompartimentMapper : Profile
    {
        public PaginationCompartimentMapper()
        {
            CreateMap<PaginationResponse<CompartimentEntity>, PaginationResponseCompartimentDto<CompartimentDto>>().ReverseMap();
        }
    }
}
