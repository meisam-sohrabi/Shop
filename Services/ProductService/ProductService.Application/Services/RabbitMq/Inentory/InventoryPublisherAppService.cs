using Microsoft.Extensions.Logging;
using ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductService.Application.Services.RabbitMq.Inentory
{
    public class InventoryPublisherAppService : IInventoryPublisherAppService
    {
        private readonly ILogger<InventoryPublisherAppService> _logger;

        public InventoryPublisherAppService(ILogger<InventoryPublisherAppService> logger)
        {
            _logger = logger;
        }
        public async Task<bool> PublishMessage<T>(T value)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using var connection = await factory.CreateConnectionAsync();

                using var channel = await connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(exchange: "Inventory-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                await channel.QueueDeclareAsync(queue: "Inventory-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                await channel.QueueBindAsync(queue: "Inventory-Queue", exchange: "Inventory-Exchange", routingKey: "Inventory-RoutingKey", arguments: null);

                var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));

                var properties = new BasicProperties { Persistent = true };

                await channel.BasicPublishAsync(exchange: "Inventory-Exchange", routingKey: "Inventory-RoutingKey", mandatory: true, basicProperties: properties, body: messageBody);

                await channel.CloseAsync();

                await connection.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("There is problem with publishing side..");
                return false;
            }

        }
    }
}
