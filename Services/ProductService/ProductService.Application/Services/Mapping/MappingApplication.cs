using AutoMapper;
using ProductService.ApplicationContract.DTO.Category;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.ProductBrand;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services.Mapping
{
    public class MappingApplication : Profile
    {
        public MappingApplication()
        {
            CreateMap<CategoryDto, CategoryEntity>();
            CreateMap<ProductRequestDto, ProductEntity>();
            CreateMap<ProductBrandDto, ProductBrandEntity>();
            CreateMap<ProductRequestDto,ProductEntity>();
        }
    }
}
