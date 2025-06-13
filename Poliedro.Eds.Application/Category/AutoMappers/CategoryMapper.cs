using AutoMapper;
using Poliedro.Eds.Application.Category.Commands.CreateCategory;
using Poliedro.Eds.Application.Category.Commands.UpdateCategory;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Domain.Category.Entities;

namespace Poliedro.Eds.Application.Category.AutoMappers;
public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
        CreateMap<CategoryEntity, CreateCategoryCommand>().ReverseMap();
        CreateMap<CategoryEntity, CreateCategoryRequestDto>().ReverseMap();
        CreateMap<CategoryEntity, UpdateCategoryCommand>().ReverseMap();
    }
}
