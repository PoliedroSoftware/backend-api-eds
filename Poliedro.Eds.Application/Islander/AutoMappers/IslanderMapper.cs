using AutoMapper;
using Poliedro.Eds.Application.Islander.Commands.CreateIslander;
using Poliedro.Eds.Application.Islander.Commands.UpdateIslander;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Application.Islander.AutoMappers;
public class IslanderMapper : Profile
{
    public IslanderMapper()
    {
        CreateMap<IslanderEntity, IslanderDto>().ReverseMap();
        CreateMap<IslanderEntity, CreateIslanderCommand>().ReverseMap();
        CreateMap<IslanderEntity, CreateIslanderRequestDto>().ReverseMap();
        CreateMap<IslanderEntity, UpdateIslanderCommand>().ReverseMap();
    }
}
