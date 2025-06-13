using AutoMapper;
using Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;
using Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.AutoMappers;
public class CompartimentMapper : Profile
{
    public CompartimentMapper()
    {
        CreateMap<CompartimentEntity, CompartimentDto>().ReverseMap();
        CreateMap<CompartimentEntity, CreateCompartimentCommand>().ReverseMap();
        CreateMap<CompartimentEntity, CreateCompartimentRequestDto>().ReverseMap();
        CreateMap<CompartimentEntity, UpdateCompartimentCommand>().ReverseMap();
    }
}
