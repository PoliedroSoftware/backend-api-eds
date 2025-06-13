using AutoMapper;
using Poliedro.Eds.Application.Provider.Commands.CreateProvider;
using Poliedro.Eds.Application.Provider.Commands.UpdateProvider;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Provider.AutoMappers;

public class ProviderMapper : Profile
{
    public ProviderMapper()
    {

        CreateMap<ProviderEntity, ProviderDto>().ReverseMap();
        CreateMap<ProviderEntity, CreateProviderCommand>().ReverseMap();
        CreateMap<ProviderEntity, CreateProviderRequestDto>().ReverseMap();
        CreateMap<ProviderEntity, UpdateProviderCommand>().ReverseMap();
    }
}