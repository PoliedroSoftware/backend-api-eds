using System;
using AutoMapper;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.StrongBox.AutoMapper;

public class StrongBoxProfile : Profile
{
    public StrongBoxProfile()
    {
        CreateMap<StrongBoxEntity, StrongBoxDto>().ReverseMap();
        CreateMap<StrongBoxEntity, StrongBoxDtoCreateRequest>();
        CreateMap<StrongBoxDtoCreateRequest, StrongBoxEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        CreateMap<StrongBoxCreateCommand, StrongBoxEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
