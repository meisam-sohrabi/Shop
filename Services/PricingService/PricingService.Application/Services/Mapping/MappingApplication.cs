using AutoMapper;
using PricingService.ApplicationContract.DTO.ProductPrice;
using PricingService.Domain.Entities;

namespace PricingService.Application.Services.Mapping
{
    public class MappingApplication : Profile
    {
        public MappingApplication()
        {
            CreateMap<ProductPriceRequestDto, ProductPriceEntity>();
        }
    }
}
