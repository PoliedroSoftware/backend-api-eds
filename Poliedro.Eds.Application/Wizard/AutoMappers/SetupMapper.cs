using AutoMapper;
using Poliedro.Eds.Application.Eds.Commands.CreateEds;
using Poliedro.Eds.Application.Eds.Commands.UpdateEds;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Application.Wizard.Commands.CreateSetup;
using Poliedro.Eds.Application.Wizard.Dtos;
using Poliedro.Eds.Domain.Wizard.Entities;

namespace Poliedro.Eds.Application.Wizard.AutoMappers;

public class SetupMapper : Profile
{
    public SetupMapper()
    {
        CreateMap<SetupEntity, SetupDto>().ReverseMap();
        CreateMap<SetupEntity, CreateSetupCommand>().ReverseMap();
        CreateMap<SetupEntity, CreateSetupRequestDto>().ReverseMap();
    }
}
