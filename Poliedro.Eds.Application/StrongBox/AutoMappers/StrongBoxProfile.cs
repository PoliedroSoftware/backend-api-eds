using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.StrongBox.AutoMappers;

public class StrongBoxProfile : Profile
{
    public StrongBoxProfile()
    {
        CreateMap<Poliedro.Eds.Domain.StrongBox.Entities.StrongBoxEntity, Poliedro.Eds.Application.StrongBox.Dtos.StrongBoxDto>().ReverseMap();
        CreateMap<StrongBoxEntity, StrongBoxTotalBalanceDto>().ReverseMap();
        CreateMap<StrongBoxEntity, StrongBoxCreateCommand>().ReverseMap();
    }
}
