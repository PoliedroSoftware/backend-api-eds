using AutoMapper;
using Poliedro.Eds.Application.Tank.Commands.CreateTank;
using Poliedro.Eds.Application.Tank.Commands.UpdateTank;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.AutoMappers;
public class TankMapper : Profile
{
    public TankMapper()
    {
        CreateMap<TankEntity, TankDto>().ReverseMap();
        CreateMap<TankEntity, CreateTankCommand>().ReverseMap();
        CreateMap<TankEntity, CreateTankRequestDto>().ReverseMap();
        CreateMap<TankEntity, UpdateTankCommand>().ReverseMap();
    }
}
