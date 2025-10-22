using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using Poliedro.Eds.Domain.RegisterShift.Entities;

namespace Poliedro.Eds.Application.RegisterShift.AutoMappers;

public class RegisterShiftMapper : Profile
{
    public RegisterShiftMapper()
    {
        CreateMap<RegisterShiftEntity, RegisterShiftDto>.ReverseMap();
    }
}
