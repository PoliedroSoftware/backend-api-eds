using AutoMapper;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Provider.AutoMappers;

public class PaginationProviderMapper : Profile
{
    public PaginationProviderMapper()
    {
        CreateMap<PaginationResponse<ProviderEntity>, PaginationResponseDto<ProviderDto>>().ReverseMap();
    }
}