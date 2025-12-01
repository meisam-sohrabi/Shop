using BaseConfig;
using MassTransit;
using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces;
using PricingService.InfrastructureContract.Interfaces.Command.ProductPrice;

namespace PricingService.Application.Services.PriceConsumer
{
    public class PriceInsertConsumer : IConsumer<PriceEventDto>
    {
        private readonly IProductPriceCommandRepository _productPriceCommandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PriceInsertConsumer(IProductPriceCommandRepository productPriceCommandRepository
            , IUnitOfWork unitOfWork)
        {
            _productPriceCommandRepository = productPriceCommandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<PriceEventDto> context)
        {
            try
            {
                var entity = new ProductPriceEntity
                {
                    ProductDetailId = context.Message.ProductDetailId,
                    Price = context.Message.Price,
                    CreateBy = context.Message.UserId
                };

                await _productPriceCommandRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There is  a problem in priceinserting consumer:{ex.Message}");
            }
        }
    }
}
