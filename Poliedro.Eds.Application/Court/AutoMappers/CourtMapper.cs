using AutoMapper;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Application.Court.Commands.UpdateCourt;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;

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
        }
    }
}
