using AutoMapper;
using Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;
using Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Application.ProductCompartiment.AutoMappers;

public class ProductCompartimentMapper : Profile
{
    public ProductCompartimentMapper()
    {
        CreateMap<ProductCompartimentEntity, ProductCompartimentDto>().ReverseMap();
        CreateMap<ProductCompartimentEntity, CreateProductCompartimentCommand>().ReverseMap();
        CreateMap<ProductCompartimentEntity, CreateProductCompartimentRequestDto>().ReverseMap();
        CreateMap<ProductCompartimentEntity, UpdateProductCompartimentCommand>().ReverseMap();
    }
}
