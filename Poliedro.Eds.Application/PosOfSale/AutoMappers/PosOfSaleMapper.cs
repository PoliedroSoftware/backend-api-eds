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
        CreateMap<PosOfSaleEntity, CreatePosOfSaleRequestDto>().ReverseMap();
        CreateMap<PosOfSaleEntity, UpdatePosOfSaleCommand>().ReverseMap();
    }
}
