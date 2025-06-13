using AutoMapper;
using Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory;
using Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.AutoMappers;
public class HoseHistoryMapper : Profile
{
    public HoseHistoryMapper()
    {
        CreateMap<HoseHistoryEntity, HoseHistoryDto>().ReverseMap();
        CreateMap<HoseHistoryEntity, CreateHoseHistoryCommand>().ReverseMap();
        CreateMap<HoseHistoryEntity, CreateHoseHistoryRequestDto>().ReverseMap();
        CreateMap<HoseHistoryEntity, UpdateHoseHistoryCommand>().ReverseMap();
    }
}
