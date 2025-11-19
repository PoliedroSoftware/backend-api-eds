using AutoMapper;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.Court;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDashboardView;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CapacityDashboardView;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using CompartmentDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.CompartmentDto;
using EdsDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.EdsDto;
using TankDto = Poliedro.Eds.Application.DashboardPowerBI.Dtos.TankDto;


namespace Poliedro.Eds.Application.DashboardPowerBI.AutoMappers;

public class DashboardPowerBIMapper : Profile
{
    public DashboardPowerBIMapper()
    {
        // Mapping for v_d_business (new independent view)
        CreateMap<BusinessDashboardViewEntity, BusinessDashboardViewDto>()
             .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
             .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.BusinessName))
             .ForMember(dest => dest.EdsName, opt => opt.MapFrom(src => src.EdsName))
             .ForMember(dest => dest.TankNumber, opt => opt.MapFrom(src => src.TankNumber))
             .ForMember(dest => dest.CompartimentNumber, opt => opt.MapFrom(src => src.CompartimentNumber.ToString()))
             .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
             .ForMember(dest => dest.ProductDate, opt => opt.MapFrom(src => src.ProductDate.ToString("yyyy-MM-dd")));

        // Mapping for v_d_capacity (new independent view)
        CreateMap<CapacityDashboardViewEntity, CapacityDashboardViewDto>()
             .ForMember(dest => dest.IdCapacity, opt => opt.MapFrom(src => src.IdCapacity.ToString()))
             .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
             .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
             .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
             .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height.ToString()))
             .ForMember(dest => dest.Gallon, opt => opt.MapFrom(src => src.Gallon.ToString()))
             .ForMember(dest => dest.Liters, opt => opt.MapFrom(src => src.Liters.ToString()))
             .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));

        CreateMap<BusinessViewEntity, Business2Dto>()
             .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
             .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.BusinessName))
             .ForMember(dest => dest.EdsName, opt => opt.MapFrom(src => src.EdsName))
             .ForMember(dest => dest.TankNumber, opt => opt.MapFrom(src => src.TankNumber))
             .ForMember(dest => dest.CompartimentNumber, opt => opt.MapFrom(src => src.CompartimentNumber.ToString()))
             .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
             .ForMember(dest => dest.ProductDate, opt => opt.MapFrom(src => src.ProductDate));


        CreateMap<CapacityViewEntity, CapacityDto>()
            .ForMember(dest => dest.IdCapacity, opt => opt.MapFrom(src => src.IdCapacity.ToString()))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Gallon, opt => opt.MapFrom(src => src.Gallon))
            .ForMember(dest => dest.Liters, opt => opt.MapFrom(src => src.Liters))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<CompartimentViewEntity, CompartimentDto>()
            .ForMember(dest => dest.IdCompartment, opt => opt.MapFrom(src => src.IdCompartment.ToString()))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
            .ForMember(dest => dest.Operative, opt => opt.MapFrom(src => src.Operative))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.IdTank, opt => opt.MapFrom(src => src.IdTank.ToString()))
            .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<EdsViewEntity, EdsDto>()
            .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()))
            .ForMember(dest => dest.EdsName, opt => opt.MapFrom(src => src.EdsName))
            .ForMember(dest => dest.Nit, opt => opt.MapFrom(src => src.Nit))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Sicom, opt => opt.MapFrom(src => src.Sicom))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
            .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.BusinessName))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));

        CreateMap<ShoppingProductViewEntity, ShoppingProductViewDto>()
            .ForMember(dest => dest.IdShoppingProduct, opt => opt.MapFrom(src => src.IdShoppingProduct.ToString()))
            .ForMember(dest => dest.IdShopping, opt => opt.MapFrom(src => src.IdShopping.ToString()))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.SellPrice, opt => opt.MapFrom(src => src.SellPrice))
            .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(src => src.PurchasePrice))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.IdCompartment, opt => opt.MapFrom(src => src.IdCompartment.ToString()))
            .ForMember(dest => dest.TotalSellPrice, opt => opt.MapFrom(src => src.TotalSellPrice))
            .ForMember(dest => dest.TotalPurchasePrice, opt => opt.MapFrom(src => src.TotalPurchasePrice))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()));

        CreateMap<Domain.Inventory.Dto.View.InventoryListResponseDto, InventoryDto>();

        CreateMap<Domain.Inventory.Dto.View.BusinessDto, Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDto>()
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()));

        CreateMap<Domain.Inventory.Dto.View.EdsDto, EdssDto>()
            .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<Domain.Inventory.Dto.View.TankDto, TankDto>()
            .ForMember(dest => dest.IdTank, opt => opt.MapFrom(src => src.IdTank.ToString()));

        CreateMap<Domain.Inventory.Dto.View.CompartmentDto, CompartmentDto>()
            .ForMember(dest => dest.IdCompartment, opt => opt.MapFrom(src => src.IdCompartment.ToString()))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()));

        CreateMap<ProductViewEntity, ProductDto>()
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
            .ForMember(dest => dest.IdProductType, opt => opt.MapFrom(src => src.IdProductType.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<ProviderViewEntity, ProviderDto>()
            .ForMember(dest => dest.IdProvider, opt => opt.MapFrom(src => src.IdProvider.ToString()))
            .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
            .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<ShoppingEntity, ShoppingDto>()
           .ForMember(dest => dest.IdShopping, opt => opt.MapFrom(src => src.IdShopping.ToString()))
           .ForMember(dest => dest.IdProvider, opt => opt.MapFrom(src => src.IdProvider.ToString()))
           .ForMember(dest => dest.IdCategory, opt => opt.MapFrom(src => src.IdCategory.ToString()));

        CreateMap<TypeOfCollectionViewEntity, TypeOfCollectionDto>()
           .ForMember(dest => dest.IdTypeOfCollection, opt => opt.MapFrom(src => src.IdTypeOfCollection.ToString()))
           .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct.ToString()))
           .ForMember(dest => dest.IdBusiness, opt => opt.MapFrom(src => src.IdBusiness.ToString()))
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<CourtListResponseEntity, Dtos.Court.CourtListResponseDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<CourtCollectionViewEntity, Dtos.Court.CourtCollectionViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtDispenserViewEntity, Dtos.Court.CourtDispenserViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.IdEds, opt => opt.MapFrom(src => src.IdEds.ToString()));

        CreateMap<CourtDocumentViewEntity, Dtos.Court.CourtDocumentViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtExpenditureViewEntity, Dtos.Court.CourtExpenditureViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<CourtEntity, CourtDto>().ReverseMap();
        CreateMap<CourtDispenserEntity, CourtDispenserDto>().ReverseMap();
        CreateMap<DocumentEntity, DocumentDto>().ReverseMap();
        CreateMap<CourtExpenditureEntity, CourtExpenditureDto>().ReverseMap();
        CreateMap<CourtTypeOfCollectionEntity, CourtTypeOfCollectionDto>().ReverseMap();
        CreateMap<CourtDispenserEntity, CourtDispenserSaleEntity>()
         .ForMember(dest => dest.IdCompartiment, opt => opt.MapFrom(src => src.IdCompartiment))
         .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct));
        CreateMap<CourtDispenserCommand, CourtDispenserSaleEntity>()
         .ForMember(dest => dest.GallonsDifferenceResult, opt => opt.MapFrom(src => src.GallonsDifferenceResult));


    }
}
