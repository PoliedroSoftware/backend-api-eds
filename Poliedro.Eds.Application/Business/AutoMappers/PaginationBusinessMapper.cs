using AutoMapper;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Business.AutoMappers;

public class PaginationBusinessMapper : Profile
{
    public PaginationBusinessMapper()
    {
        CreateMap<PaginationResponse<BusinessEntity>, PaginationResponseDto<BusinessDto>>().ReverseMap();
    }
}
