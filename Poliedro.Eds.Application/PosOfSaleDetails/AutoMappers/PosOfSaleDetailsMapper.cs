using AutoMapper;
using Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale;
using Poliedro.Eds.Application.PosOfSaleDetails.Commands.UpdatePosOfSale;
using Poliedro.Eds.Domain.PosOfSaleDetails.Dtos;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.AutoMappers;

public class PosOfSaleDetailsMapper : Profile
{
    public PosOfSaleDetailsMapper()
    {
        CreateMap<PosOfSaleDetailsEntity, PosOfSaleDetailsDto>().ReverseMap();
        CreateMap<PosOfSaleDetailsEntity, CreatePosOfSaleDetailsCommand>().ReverseMap();
        CreateMap<PosOfSaleDetailsEntity, CreatePosOfSaleDetailsRequestDto>().ReverseMap();
        CreateMap<PosOfSaleDetailsEntity, UpdatePosOfSaleDetailsCommand>().ReverseMap();
    }
}
