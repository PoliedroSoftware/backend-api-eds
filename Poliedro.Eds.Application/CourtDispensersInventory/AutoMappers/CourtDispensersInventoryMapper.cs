using AutoMapper;
using Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory;
using Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.AutoMappers;
public class CourtDispensersInventoryMapper : Profile
{
    public CourtDispensersInventoryMapper()
    {
        CreateMap<CourtDispensersInventoryEntity, CourtDispensersInventoryDto>().ReverseMap();
        CreateMap<CourtDispensersInventoryEntity, CreateCourtDispensersInventoryCommand>().ReverseMap();
        CreateMap<CourtDispensersInventoryEntity, CreateCourtDispensersInventoryRequestDto>().ReverseMap();
        CreateMap<CourtDispensersInventoryEntity, UpdateCourtDispensersInventoryCommand>().ReverseMap();
    }
}
