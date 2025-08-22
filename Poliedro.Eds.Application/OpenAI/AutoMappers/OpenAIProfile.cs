using AutoMapper;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Application.OpenAI.AutoMappers;

public class OpenAIProfile : Profile
{
    public OpenAIProfile()
    {
        CreateMap<OpenAIResponseEntity, OpenAIResponseDto>();
    }
}