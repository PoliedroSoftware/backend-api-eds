using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Poliedro.Eds.Application.StrongBox.AutoMapper;

public class StrongBoxProfile : Profile
{
    public StrongBoxProfile()
    {
        CreateMap<Domain.StrongBox.StrongBox, StrongBoxDto>();
        CreateMap<StrongBoxDto, Domain.StrongBox.StrongBox>();
    }
}
