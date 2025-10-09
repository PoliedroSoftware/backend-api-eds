using AutoMapper;
using Poliedro.Eds.Application.Account.Dtos;
using Poliedro.Eds.Domain.Account.Entities;

namespace Poliedro.Eds.Application.Account.AutoMappers;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<AccountEntity, AccountDto>();
        CreateMap<AccountDto, AccountEntity>();
        CreateMap<AccountCreateDto, AccountEntity>();
    }
}
