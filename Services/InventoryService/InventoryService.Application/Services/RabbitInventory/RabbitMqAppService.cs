//using InventoryService.ApplicationContract.DTO.ProductInventory;
//using InventoryService.ApplicationContract.Interfaces.ProductInventory;
//using InventoryService.ApplicationContract.Interfaces.RabbitMq;
//using Microsoft.Extensions.DependencyInjection;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System.Text;
//using System.Text.Json;

//namespace InventoryService.Application.Services.RabbitInventory
//{
//    public class RabbitMqAppService : IRabbitMqAppService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;

//        public RabbitMqAppService(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }
//        public async Task GetMessage<T>(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                var factory = new ConnectionFactory() { HostName = "localhost" };

//                using var connection = await factory.CreateConnectionAsync();

//                using var channel = await connection.CreateChannelAsync();

//                await channel.ExchangeDeclareAsync(exchange: "PlaceOrder-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

//                await channel.QueueDeclareAsync(queue: "PlaceOrder-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

//                await channel.QueueBindAsync(queue: "PlaceOrder-Queue", exchange: "PlaceOrder-Exchange", routingKey: "PlaceOrder-RoutingKey", arguments: null);

//                var consumer = new AsyncEventingBasicConsumer(channel);

//                consumer.ReceivedAsync += async (model, ea) =>
//                {
//                    var body = ea.Body.ToArray();
//                    var message = Encoding.UTF8.GetString(body);
//                    var data = JsonSerializer.Deserialize<T>(message);

                    
//                    using (var scope = _scopeFactory.CreateScope())
//                    {
//                        var appService = scope.ServiceProvider.GetRequiredService<IProductInventoryAppService>();
//                        await appService.CreateProductInventory(data as ProductInventoryRequestDto);
//                    }

//                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
//                };
//                await channel.BasicConsumeAsync(queue: "PlaceOrder-Queue", autoAck: false, consumer: consumer);

//                await Task.Delay(Timeout.Infinite, cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                Console.WriteLine("There is problem with consumer side..");
//            }

//        }

//        public async Task PublishMessage<T>(T value)
//        {
//            try
//            {
//                var factory = new ConnectionFactory() { HostName = "localhost" };

//                using var connection = await factory.CreateConnectionAsync();

//                using var channel = await connection.CreateChannelAsync();

//                await channel.ExchangeDeclareAsync(exchange: "UpdateInventory-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

//                await channel.QueueDeclareAsync(queue: "UpdateInventory-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

//                await channel.QueueBindAsync(queue: "UpdateInventory-Queue", exchange: "UpdateInventory-Exchange", routingKey: "UpdateInventory-RoutingKey", arguments: null);

//                var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));

//                var properties = new BasicProperties { Persistent = true };

//                await channel.BasicPublishAsync(exchange: "UpdateInventory-Exchange", routingKey: "UpdateInventory-RoutingKey", mandatory: true, basicProperties: properties, body: messageBody);

//                await channel.CloseAsync();

//                await connection.CloseAsync();

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("There is problem with publishing side..");
//            }

//        }
//    }
//}
