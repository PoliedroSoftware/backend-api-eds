using AutoMapper;
using Poliedro.Eds.Application.Business.Commands.CreateBusiness;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Application.Business.AutoMappers;

public class BusinessMapper : Profile
{
    public BusinessMapper()
    {

        CreateMap<BusinessEntity, BusinessDto>().ReverseMap();
        CreateMap<BusinessEntity, CreateBusinessCommand>().ReverseMap();
        CreateMap<BusinessEntity, CreateBusinessRequestDto>().ReverseMap();
        CreateMap<BusinessEntity, UpdateBusinessCommand>().ReverseMap();
    }
}