//using Microsoft.Extensions.Hosting;
//using ProductService.ApplicationContract.RabbitMq.Price;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//namespace ProductService.Application.Services.RabbitMq.Price
//{
//    public class RabbitPriceConsumerAppService : BackgroundService, IRabbitPriceConsumerAppService
//    {
//        public async Task GetMessage<T>(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                var factory = new ConnectionFactory() { HostName = "localhost" };

//                using var connection = await factory.CreateConnectionAsync();

//                using var channel = await connection.CreateChannelAsync();

//                await channel.ExchangeDeclareAsync(exchange: "Price-Consume-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

//                await channel.QueueDeclareAsync(queue: "Price-Consume-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

//                await channel.QueueBindAsync(queue: "Price-Consume-Queue", exchange: "Price-Consume-Exchange", routingKey: "Price-Consume-RoutingKey", arguments: null);

//                var consumer = new AsyncEventingBasicConsumer(channel);

//                consumer.ReceivedAsync += async (model, ea) =>
//                {
//                    var body = ea.Body.ToArray();
//                    var message = Encoding.UTF8.GetString(body);
//                    var data = JsonSerializer.Deserialize<T>(message) as ProductInventoryResponseDto;
//                    Console.WriteLine($"Data is updated order date is :{data.ChangeDate}");
//                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
//                };

//                await channel.BasicConsumeAsync(queue: "Price-Consume-Queue", autoAck: false, consumer: consumer);

//                await Task.Delay(Timeout.Infinite, cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogInformation(ex.Message);
//                _logger.LogError("There is problem with consumer side..");
//            }

//        }



//        public override Task StartAsync(CancellationToken cancellationToken)
//        {
//            return base.StartAsync(cancellationToken);
//        }
//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            throw new NotImplementedException();
//        }

//        public override Task StopAsync(CancellationToken cancellationToken)
//        {
//            return base.StopAsync(cancellationToken);
//        }
//    }
//}
