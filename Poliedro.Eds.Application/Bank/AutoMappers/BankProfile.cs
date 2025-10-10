using AutoMapper;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Application.Bank.AutoMappers;

public class BankProfile : Profile
{
    public BankProfile()
    {
        CreateMap<BankEntity, BankDto>();
        CreateMap<BankDto, BankEntity>();
        CreateMap<BankDtoCreateRequest, BankEntity>();
    }
}
