using AutoMapper;
using Poliedro.Eds.Application.Phone.Commands.CreatePhone;
using Poliedro.Eds.Application.Phone.Commands.UpdatePhone;
using Poliedro.Eds.Application.Phone.Dtos;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Application.Phone.AutoMappers;

public class PhoneMapper : Profile
{
    public PhoneMapper()
    {
        CreateMap<PhoneEntity, PhoneDto>().ReverseMap();
        CreateMap<PhoneEntity, CreatePhoneCommand>().ReverseMap();
        CreateMap<PhoneEntity, CreatePhoneRequestDto>().ReverseMap();
        CreateMap<PhoneEntity, UpdatePhoneCommand>().ReverseMap();
    }
}
