using AutoMapper;
using Poliedro.Eds.Application.CompartimentCapacity.Commands.CreateCompartimentCapacity;
using Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Application.CompartimentCapacity.AutoMappers;

public class CompartimentCapacityMapper : Profile
{
    public CompartimentCapacityMapper()
    {
        CreateMap<CompartimentCapacityEntity, CompartimentCapacityDto>().ReverseMap();
        CreateMap<CompartimentCapacityEntity, CreateCompartimentCapacityCommand>().ReverseMap();
        CreateMap<CompartimentCapacityEntity, CreateCompartimentCapacityRequestDto>().ReverseMap();
        CreateMap<CompartimentCapacityEntity, UpdateCompartimentCapacityCommand>().ReverseMap();
    
    }
}