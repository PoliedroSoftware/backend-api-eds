using AutoMapper;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Application.TransferValidation.AutoMappers;

public class TransferValidationProfile : Profile
{
    public TransferValidationProfile()
    {
        CreateMap<TransferValidationEntity, TransferValidationDto>();
        
        CreateMap<TransferValidationDto, TransferValidationEntity>();
        
        CreateMap<TransferValidationCreateRequestDto, TransferValidationEntity>();
    }
}
