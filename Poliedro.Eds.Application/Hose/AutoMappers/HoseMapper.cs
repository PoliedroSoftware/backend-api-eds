using AutoMapper;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Application.Hose.AutoMappers;
public class HoseMapper : Profile
{
    public HoseMapper()
    {
        CreateMap<HoseEntity, HoseDto>().ReverseMap();
        CreateMap<LastAccumulatedEntity, LastAccumulatedDto>().ReverseMap();
        CreateMap<HoseEntity, CreateHoseCommand>().ReverseMap();
        CreateMap<HoseEntity, CreateHoseRequestDto>().ReverseMap();
        CreateMap<HoseEntity, UpdateHoseCommand>().ReverseMap();
    }
}
