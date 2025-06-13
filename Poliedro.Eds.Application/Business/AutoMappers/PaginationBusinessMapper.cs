using AutoMapper;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Application.Business.AutoMappers;

public class PaginationBusinessMapper : Profile
{
    public PaginationBusinessMapper()
    {
        CreateMap<PaginationResponse<BusinessEntity>, PaginationResponseDto<BusinessDto>>().ReverseMap();
    }
}