using AutoMapper;
using Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers;
using Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.AutoMappers;
public class DispensersMapper : Profile
{
    public DispensersMapper()
    {
        CreateMap<DispensersEntity, DispensersDto>().ReverseMap();
        CreateMap<DispensersEntity, CreateDispensersCommand>().ReverseMap();
        CreateMap<DispensersEntity, CreateDispensersRequestDto>().ReverseMap();
        CreateMap<DispensersEntity, UpdateDispensersCommand>().ReverseMap();
    }
}
