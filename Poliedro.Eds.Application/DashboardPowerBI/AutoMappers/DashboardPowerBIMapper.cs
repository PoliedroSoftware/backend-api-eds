
using AutoMapper;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Capacity.Entities;

using BusinessDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDto;
using EdsDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.EdsDto;
using TankDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.TankDto;
using CompartmentDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.CompartmentDto;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.Court;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;


namespace Poliedro.Eds.Application.DashboardPowerBI.AutoMappers;

public class DashboardPowerBIMapper: Profile
{
    public DashboardPowerBIMapper()
    {
        

        CreateMap<CapacityEntity, CapacityDto>()
            .ForMember(dest => dest.IdCapacity, opt => opt.MapFrom(src => src.IdCapacity.ToString()))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Gallon, opt => opt.MapFrom(src => src.Gallon))
            .ForMember(dest => dest.Liters, opt => opt.MapFrom(src => src.Liters));

        CreateMap<CompartimentEntity, CompartimentDto>()
            .ForMember(dest => dest.IdCompartment, opt => opt.MapFrom(src => src.IdCompartment.ToString()))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
            .ForMember(dest => dest.Operative, opt => opt.MapFrom(src => src.Operative))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.IdTank, opt => opt.MapFrom(src => src.IdTank.ToString()));

        CreateMap<EdsEntity, EdsDto>()
          .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Nit, opt => opt.MapFrom(src => src.Nit))
          .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
          .ForMember(dest => dest.Sicom, opt => opt.MapFrom(src => src.Sicom))
          .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()));

        CreateMap<Domain.Inventory.Dto.View.InventoryListResponseDto, InventoryDto>();

        CreateMap<Domain.Inventory.Dto.View.BusinessDto, BusinessDto>()
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()));

        CreateMap<Domain.Inventory.Dto.View.EdsDto, EdssDto>()
            .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<Domain.Inventory.Dto.View.TankDto, TankDto>()
            .ForMember(dest => dest.IdTank, opt => opt.MapFrom(src => src.IdTank.ToString()));

        CreateMap<Domain.Inventory.Dto.View.CompartmentDto, CompartmentDto>()
            .ForMember(dest => dest.IdCompartment, opt => opt.MapFrom(src => src.IdCompartment.ToString()))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()));

        CreateMap<ProductEntity, ProductDto>()
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.IdProductType, opt => opt.MapFrom(src => src.IdProductType.ToString()));

        CreateMap<ProviderEntity, ProviderDto>()
            .ForMember(dest => dest.IdProvider, opt => opt.MapFrom(src => src.IdProvider.ToString()));

        CreateMap<ShoppingEntity, ShoppingDto>()
           .ForMember(dest => dest.IdShopping, opt => opt.MapFrom(src => src.IdShopping.ToString()))
           .ForMember(dest => dest.IdProvider, opt => opt.MapFrom(src => src.IdProvider.ToString()))
           .ForMember(dest => dest.IdCategory, opt => opt.MapFrom(src => src.IdCategory.ToString()));

        CreateMap<TypeOfCollectionEntity, TypeOfCollectionDto>()
           .ForMember(dest => dest.IdTypeOfCollection, opt => opt.MapFrom(src => src.IdTypeOfCollection.ToString()));

        CreateMap<CourtListResponseDto, Dtos.Court.CourtListResponseDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<CourtCollectionViewDto, Dtos.Court.CourtCollectionViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtDispenserViewDto, Dtos.Court.CourtDispenserViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<CourtDocumentViewDto, Dtos.Court.CourtDocumentViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtExpenditureViewDto, Dtos.Court.CourtExpenditureViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtEntity, CourtDto>().ReverseMap();
        CreateMap<CourtDispenserEntity, CourtDispenserDto>().ReverseMap();
        CreateMap<DocumentEntity, DocumentDto>().ReverseMap();
        CreateMap<CourtExpenditureEntity, CourtExpenditureDto>().ReverseMap();
        CreateMap<CourtTypeOfCollectionEntity, CourtTypeOfCollectionDto>().ReverseMap();
        CreateMap<CourtDispenserEntity, ICourtDispenserSaleEntity>()
         .ForMember(dest => dest.IdCompartiment, opt => opt.MapFrom(src => src.IdCompartiment))
         .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct));
        CreateMap<CourtDispenserCommand, ICourtDispenserSaleEntity>()
         .ForMember(dest => dest.GallonsDifferenceResult, opt => opt.MapFrom(src => src.GallonsDifferenceResult));


    }
}
