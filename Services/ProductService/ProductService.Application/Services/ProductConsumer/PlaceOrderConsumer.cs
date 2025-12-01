using BaseConfig;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using ProductService.ApplicationContract.DTO.ProductDetail;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
namespace ProductService.Application.Services.ProductConsumer
{
    public sealed class PlaceOrderConsumer : IConsumer<ProductDetailEventDto>
    {
        private readonly IProductDetailCommandRepository _productDetailCommandRepository;
        private readonly IProductDetailQueryRepository _productDetailQueryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceOrderConsumer(IProductDetailCommandRepository productDetailCommandRepository
            , IProductDetailQueryRepository productDetailQueryRepository
            , IUnitOfWork unitOfWork)
        {
            _productDetailCommandRepository = productDetailCommandRepository;
            _productDetailQueryRepository = productDetailQueryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<ProductDetailEventDto> context)
        {
            var maxRetries = 3;
            for (var retry = 0; retry < maxRetries; retry++)
            {
                {
                    try
                    {
                        var productExist = await _productDetailQueryRepository
                             .GetQueryable()
                             .FirstOrDefaultAsync(c => c.Id == context.Message.Id);

                        productExist.Quantity -= context.Message.Quantity;
                        _productDetailCommandRepository.Edit(productExist);
                        await _unitOfWork.SaveChangesAsync();
                        return;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine($"Concurrency conflict for ProductId={context.Message.Id}. Retry {retry}/{maxRetries}", context.Message.Id, retry + 1, maxRetries);
                        // Reload the conflicting entry (EF entries) then retry loop to re-evaluate
                        foreach (var entry in ex.Entries)
                        {
                            await entry.ReloadAsync();
                        }

                        if (retry == maxRetries - 1) throw;
                        await Task.Delay(100 * (retry + 1));
                    }

                }

            }
        }
    }
}



