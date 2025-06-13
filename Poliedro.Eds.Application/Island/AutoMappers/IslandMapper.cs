using AutoMapper;
using Poliedro.Eds.Application.Island.Commands.CreateIsland;
using Poliedro.Eds.Application.Island.Commands.UpdateIsland;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Island.Entities;


namespace Poliedro.Eds.Application.Island.AutoMappers;
public class IslandMapper : Profile
{
    public IslandMapper()
    {
        CreateMap<IslandEntity, IslandDto>().ReverseMap();
        CreateMap<IslandEntity, CreateIslandCommand>().ReverseMap();
        CreateMap<IslandEntity, CreateIslandRequestDto>().ReverseMap();
        CreateMap<IslandEntity, UpdateIslandCommand>().ReverseMap();
    }
}
