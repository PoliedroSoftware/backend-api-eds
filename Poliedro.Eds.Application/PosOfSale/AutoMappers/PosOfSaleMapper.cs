using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale;
using Poliedro.Eds.Application.PosOfSale.Commands.UpdatePosOfSale;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.AutoMappers;

public class PosOfSaleMapper : Profile
{
    public PosOfSaleMapper()
    {
        CreateMap<PosOfSaleEntity, PosOfSaleDto>().ReverseMap();
        CreateMap<PosOfSaleEntity, CreatePosOfSaleCommand>().ReverseMap();
        CreateMap<PosOfSaleEntity, CreatePosOfSaleRequestDto>().ReverseMap()
            .ForMember(dest => dest.ProviderTag,
            opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.Status,
            opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.CurrencyCode,
            opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<PosOfSaleEntity, UpdatePosOfSaleCommand>().ReverseMap();
    }
}
