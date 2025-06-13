using AutoMapper;
using Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
using Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Application.Expenditures.AutoMappers;

public class ExpendituresMapper : Profile
{
    public ExpendituresMapper()
    {
        CreateMap<ExpendituresEntity, ExpendituresDto>().ReverseMap();
        CreateMap<ExpendituresEntity, CreateExpendituresCommand>().ReverseMap();
        CreateMap<ExpendituresEntity, CreateExpendituresRequestDto>().ReverseMap();
        CreateMap<ExpendituresEntity, UpdateExpendituresCommand>().ReverseMap();
    
    }
}
