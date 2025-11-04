namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Price
{
    public interface IRabbitPricePublisherAppService
    {
        Task<bool> PublishMessage<T>(T value);
    }
}
