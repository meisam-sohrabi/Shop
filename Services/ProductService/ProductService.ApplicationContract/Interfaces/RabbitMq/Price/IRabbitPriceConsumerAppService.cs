namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Price
{
    public interface IRabbitPriceConsumerAppService
    {
        Task GetMessage<T>(CancellationToken cancellationToken = default);
    }
}
