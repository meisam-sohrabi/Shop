using Microsoft.Extensions.Logging;
using ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductService.Application.Services.RabbitMq.Inentory
{
    public class RabbitInventoryPublisherAppService : IRabbitInventoryPublisherAppService
    {
        private readonly ILogger<RabbitInventoryPublisherAppService> _logger;

        public RabbitInventoryPublisherAppService(ILogger<RabbitInventoryPublisherAppService> logger)
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

                await channel.ExchangeDeclareAsync(exchange: "PlaceOrder-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                await channel.QueueDeclareAsync(queue: "PlaceOrder-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                await channel.QueueBindAsync(queue: "PlaceOrder-Queue", exchange: "PlaceOrder-Exchange", routingKey: "PlaceOrder-RoutingKey", arguments: null);

                var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));

                var properties = new BasicProperties { Persistent = true };

                await channel.BasicPublishAsync(exchange: "PlaceOrder-Exchange", routingKey: "PlaceOrder-RoutingKey", mandatory: true, basicProperties: properties, body: messageBody);

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
