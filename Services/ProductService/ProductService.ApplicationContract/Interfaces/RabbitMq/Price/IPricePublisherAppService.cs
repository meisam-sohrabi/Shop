namespace ProductService.ApplicationContract.Interfaces.RabbitMq.Price
{
    public interface IPricePublisherAppService
    {
        Task<bool> PublishMessage<T>(T value);
    }
}
