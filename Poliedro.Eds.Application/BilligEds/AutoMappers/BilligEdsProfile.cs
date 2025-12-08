using AutoMapper;
using Poliedro.Eds.Application.BilligEds.Dtos;
using Poliedro.Eds.Domain.BilligEds.Entities;

namespace Poliedro.Eds.Application.BilligEds.AutoMappers;

public class BilligEdsProfile : Profile
{
    public BilligEdsProfile()
    {
        CreateMap<BillidEdsRequestEntity, BilligEdsDto>();
    }
}
