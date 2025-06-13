using AutoMapper;
using Poliedro.Eds.Application.EdsTank.Commands.CreateEdsTank;
using Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.AutoMappers;

public class EdsTankMapper : Profile
{
    public EdsTankMapper()
    {
        CreateMap<EdsTankEntity, EdsTankDto>().ReverseMap();
        CreateMap<EdsTankEntity, CreateEdsTankCommand>().ReverseMap();
        CreateMap<EdsTankEntity, CreateEdsTankRequestDto>().ReverseMap();
        CreateMap<EdsTankEntity, UpdateEdsTankCommand>().ReverseMap();
    }
}
