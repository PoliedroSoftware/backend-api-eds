using AutoMapper;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Court.Commands.UpdateCourt;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.Court.Dtos.View;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Application.Inventory.Commands;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;

namespace Poliedro.Eds.Application.Court.AutoMappers
{
    public class CourtMapper : Profile
    {
        public CourtMapper()
        {
            CreateMap<CourtEntity, CreateCourtCommand>().ReverseMap();
            CreateMap<CourtEntity, UpdateCourtCommand>().ReverseMap();
            CreateMap<CourtEntity, CourtDto>().ReverseMap();
            CreateMap<CourtDispenserEntity, CourtDispenserDto>().ReverseMap();
            CreateMap<DocumentEntity, DocumentDto>().ReverseMap();
            CreateMap<CourtExpenditureEntity, CourtExpenditureDto>().ReverseMap();
            CreateMap<CourtTypeOfCollectionEntity, CourtTypeOfCollectionDto>().ReverseMap();
            CreateMap<CourtDispenserEntity, CourtDispenserCommand>().ReverseMap();
            CreateMap<CourtExpenditureEntity, CourtExpenditureCommand>().ReverseMap();
            CreateMap<DocumentEntity, DocumentCommand>().ReverseMap();
            CreateMap<CourtExpenditureEntity, CourtExpenditureCommand>().ReverseMap();
            CreateMap<CourtTypeOfCollectionEntity, CourtTypeOfCollectionCommand>().ReverseMap();
            CreateMap<CourtDispenserEntity, ICourtDispenserSaleEntity>()
             .ForMember(dest => dest.IdCompartiment, opt => opt.MapFrom(src => src.IdCompartiment))
             .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct));
            CreateMap<CourtDispenserCommand, ICourtDispenserSaleEntity>()
             .ForMember(dest => dest.GallonsDifferenceResult, opt => opt.MapFrom(src => src.GallonsDifferenceResult));

            CreateMap<InventoryEntity, InventoryDto>().ReverseMap();
            CreateMap<InventoryEntity, InventoryCommand>().ReverseMap();

            CreateMap<CourtCollectionViewEntity, CourtCollectionViewDto>().ReverseMap();
            CreateMap<CourtDispenserViewEntity, CourtDispenserViewDto>().ReverseMap();
            CreateMap<CourtDocumentViewEntity, CourtDocumentViewDto>().ReverseMap();
            CreateMap<CourtExpenditureViewEntity, CourtExpenditureViewDto>().ReverseMap();
            CreateMap<CourtListResponseEntity, CourtListResponseDto>();
            CreateMap<CourtViewEntity, CourtViewDto>().ReverseMap();


        }
    }
}
