using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.AutoMappers;

public class PosOfSaleMapper : Profile
{
    public PosOfSaleMapper()
    {
        CreateMap<PosOfSaleEntity, PosOfSaleDto>().ReverseMap();
        CreateMap<LastAccumulatedEntity, LastAccumulatedDto>().ReverseMap();
        CreateMap<PosOfSaleEntity, CreateHoseCommand>().ReverseMap();
        CreateMap<PosOfSaleEntity, CreateHoseRequestDto>().ReverseMap();
        CreateMap<PosOfSaleEntity, UpdateHoseCommand>().ReverseMap();
    }
}
