using AutoMapper;
using Poliedro.Eds.Application.Product.Commands.CreateProduct;
using Poliedro.Eds.Application.Product.Commands.UpdateProduct;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Application.Product.AutoMappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductDto>().ReverseMap();
        CreateMap<ProductEntity, CreateProductCommand>().ReverseMap();
        CreateMap<ProductEntity, CreateProductRequestDto>().ReverseMap();
        CreateMap<ProductEntity, UpdateProductCommand>().ReverseMap();
    }
}
