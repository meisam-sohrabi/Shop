namespace InventoryService.ApplicationContract.Interfaces.RabbitMq
{
    public interface IRabbitPublisherAppService<T>
    {
        Task PublishMessage(T value);
    }
}
