namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory
{
    public interface IRabbitInventoryPublisherAppService
    {
        Task<bool> PublishMessage<T>(T value);

    }
}
