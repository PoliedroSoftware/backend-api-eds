using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.StrongBox.AutoMapper
{
    public class StrongBoxProFile : Profile
    {
        public StrongBoxProFile()
        {
            CreateMap<StrongBoxEntity, StrongBoxDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.IdCorte, opt => opt.MapFrom(src => src.IdCorte))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Ammount, opt => opt.MapFrom(src => src.Ammount))
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
            CreateMap<StrongBoxCreateCommand, StrongBoxEntity>();
            CreateMap<StrongBoxDtoCreateRequest, StrongBoxEntity>()
                .ConstructUsing(src => new StrongBoxEntity(
                    src.DateTime,
                    src.IdCorte,
                    src.Type,
                    src.Ammount,
                    0m, // El saldo se calculará en el servicio
                    src.Note));
        }
    }
}
