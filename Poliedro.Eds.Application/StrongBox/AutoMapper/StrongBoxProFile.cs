using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.StrongBox.AutoMapper;

public class StrongBoxProfile : Profile
{
    public StrongBoxProfile()
    {
        CreateMap<StrongBoxEntity, StrongBoxDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dest => dest.IdCorte, opt => opt.MapFrom(src => src.IdCorte))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Ammount, opt => opt.MapFrom(src => src.Ammount))
            .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.Saldo))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));
    }
}
