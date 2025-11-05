namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory
{
    public interface IInventoryPublisherAppService
    {
        Task<bool> PublishMessage<T>(T value);

    }
}
