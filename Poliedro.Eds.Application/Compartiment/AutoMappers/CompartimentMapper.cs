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
        CreateMap<CreateCompartimentRequestDto, CompartimentEntity>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<CompartimentEntity, CreateCompartimentRequestDto>().ReverseMap(); // Mantener el ReverseMap para otros usos si es necesario
        CreateMap<CompartimentEntity, UpdateCompartimentCommand>().ReverseMap();
    }
}
