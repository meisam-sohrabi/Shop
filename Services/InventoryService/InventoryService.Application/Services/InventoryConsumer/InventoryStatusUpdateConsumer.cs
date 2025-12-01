using BaseConfig;
using InventoryService.Domain.Entities;
using InventoryService.InfrastructureContract.Interfaces;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using MassTransit;

namespace InventoryService.Application.Services.InventoryConsumer
{
    public class InventoryStatusUpdateConsumer : IConsumer<InventoryEventDto>
    {
        private readonly IProductInventoryCommandRepository _productInventoryCommandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryStatusUpdateConsumer(IProductInventoryCommandRepository productInventoryCommandRepository
            , IUnitOfWork unitOfWork)
        {
            _productInventoryCommandRepository = productInventoryCommandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<InventoryEventDto> context)
        {

            try
            {

                var entity = new ProductInventoryEntity
                {
                    ProductDetailId = context.Message.ProductDetailId,
                    QuantityChange = context.Message.QuantityChange,
                    CreateBy = context.Message.UserId
                };

                await _productInventoryCommandRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There is a problem with consuming data in inventory servie:{ex.Message}");
            }



        }

    }
}

