using AutoMapper;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.AutoMappers;
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<PaginationResponse<TypeOfCollectionEntity>, PaginationResponseDto<TypeOfCollectionDto>>().ReverseMap();
        }
    }

