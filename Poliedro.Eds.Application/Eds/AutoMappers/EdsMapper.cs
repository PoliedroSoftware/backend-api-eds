using AutoMapper;
using Poliedro.Eds.Application.Eds.Commands.CreateEds;
using Poliedro.Eds.Application.Eds.Commands.UpdateEds;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.AutoMappers;

public class EdsMapper : Profile
{
    public EdsMapper()
    {

        CreateMap<EdsEntity, EdsDto>().ReverseMap();
        CreateMap<EdsEntity, CreateEdsCommand>().ReverseMap();
        CreateMap<EdsEntity, CreateEdsRequestDto>().ReverseMap();
        CreateMap<EdsEntity, UpdateEdsCommand>().ReverseMap();
    }
}