using AutoMapper;
using Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;
using Poliedro.Eds.Application.Capacity.Commands.UpdateCapacity;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Application.Capacity.AutoMappers;

public class CapacityMapper : Profile
{
    public CapacityMapper()
    {

        CreateMap<CapacityEntity, CapacityDto>().ReverseMap();
        CreateMap<CapacityEntity, CreateCapacityCommand>().ReverseMap();
        CreateMap<CapacityEntity, CreateCapacityRequestDto>().ReverseMap();
        CreateMap<CapacityEntity, UpdateCapacityCommand>().ReverseMap();
    }
}