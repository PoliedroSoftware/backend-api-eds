using AutoMapper;
using Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType;
using Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.AutoMappers;

public class DispenserTypeMapper : Profile
{
    public DispenserTypeMapper()
    {
        CreateMap<DispenserTypeEntity, DispenserTypeDto>().ReverseMap();
        CreateMap<DispenserTypeEntity, CreateDispenserTypeCommand>().ReverseMap();
        CreateMap<DispenserTypeEntity, CreateDispenserTypeRequestDto>().ReverseMap();
        CreateMap<DispenserTypeEntity, UpdateDispenserTypeCommand>().ReverseMap();
    }
}
