using AutoMapper;
using Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;
using Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.AutoMappers;

public class TypeOfCollectionMapper : Profile
{
    public TypeOfCollectionMapper()
    {
        CreateMap<TypeOfCollectionEntity, TypeOfCollectionDto>().ReverseMap();
        CreateMap<TypeOfCollectionEntity, CreateTypeOfCollectionCommand>().ReverseMap();
        CreateMap<TypeOfCollectionEntity, CreateTypeOfCollectionRequestDto>().ReverseMap();
        CreateMap<TypeOfCollectionEntity, UpdateTypeOfCollectionCommand>().ReverseMap();
    
    }
}