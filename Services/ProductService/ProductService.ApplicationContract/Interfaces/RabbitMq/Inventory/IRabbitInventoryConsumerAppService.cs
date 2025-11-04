namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory
{
    public interface IRabbitInventoryConsumerAppService
    {
        Task GetMessage<T>(CancellationToken cancellationToken = default);

    }
}
