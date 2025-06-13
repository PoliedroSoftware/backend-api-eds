using AutoMapper;
using Poliedro.Eds.Application.ProductType.Commands.CreateProductType;
using Poliedro.Eds.Application.ProductType.Commands.UpdateProductType;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Application.ProductType.AutoMappers;

public class ProductTypeMapper : Profile
{
    public ProductTypeMapper()
    {
        CreateMap<ProductTypeEntity, ProductTypeDto>().ReverseMap();
        CreateMap<ProductTypeEntity, CreateProductTypeCommand>().ReverseMap();
        CreateMap<ProductTypeEntity, CreateProductTypeRequestDto>().ReverseMap();
        CreateMap<ProductTypeEntity, UpdateProductTypeCommand>().ReverseMap();
    
    }
}
