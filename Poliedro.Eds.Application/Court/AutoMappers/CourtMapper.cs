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

namespace Poliedro.Eds.Application.Court.AutoMappers;

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

        // Fix the DocumentCommand to DocumentEntity mapping
        // Ignorar Descripcion para evitar guardar imágenes base64 pesadas en DB
        CreateMap<DocumentCommand, DocumentEntity>()
        .ForMember(dest => dest.Descripcion, opt => opt.Ignore()) // Ignorar para no guardar en DB
        .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.DocumentName))
        .ForMember(dest => dest.IdCourtDocument, opt => opt.Ignore()) // Auto-generated
        .ForMember(dest => dest.IdCourt, opt => opt.Ignore()); // Will be set by EF relationships

        CreateMap<DocumentEntity, DocumentCommand>()
        .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
             .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.DocumentName));

        CreateMap<CourtExpenditureEntity, CourtExpenditureCommand>().ReverseMap();
        CreateMap<CourtTypeOfCollectionEntity, CourtTypeOfCollectionCommand>().ReverseMap();
        CreateMap<CourtDispenserEntity, CourtDispenserSaleEntity>()
         .ForMember(dest => dest.IdCompartiment, opt => opt.MapFrom(src => src.IdCompartiment))
         .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.IdProduct));
        CreateMap<CourtDispenserCommand, CourtDispenserSaleEntity>()
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
