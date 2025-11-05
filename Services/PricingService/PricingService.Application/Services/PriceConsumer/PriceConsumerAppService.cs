using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using PricingService.ApplicationContract.DTO.ProductPrice;
using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces;
using PricingService.InfrastructureContract.Interfaces.Command.ProductPrice;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PricingService.Application.Services.RabbitPrice
{
    public class PriceConsumerAppService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;

        public PriceConsumerAppService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };


            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(retryCount: 3, sleepDurationProvider: attempt => TimeSpan.FromMinutes(1));

            try
            {
                await retryPolicy.Execute(async () =>
                {
                    _connection = await factory.CreateConnectionAsync();
                });

                _channel = await _connection.CreateChannelAsync();
                await _channel.ExchangeDeclareAsync(exchange: "Price-Publish-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                await _channel.QueueDeclareAsync(queue: "Price-Publish-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                await _channel.QueueBindAsync(queue: "Price-Publish-Queue", exchange: "Price-Publish-Exchange", routingKey: "Price-Publish-RoutingKey", arguments: null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL: Failed to connect to RabbitMQ after all retries. Service initialization failed: {ex.Message}");
            }

            await base.StartAsync(cancellationToken);
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var productRepo = scope.ServiceProvider.GetRequiredService<IProductPriceCommandRepository>();
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var data = JsonSerializer.Deserialize<ProductPriceRequestDto>(message);
                    if (data == null || data.ProductDetailId == 0)
                    {
                        Console.WriteLine("Received invalid or incomplete message.");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                        return;
                    }
                    var price = new ProductPriceEntity
                    {
                        Price = data.Price,
                        ProductDetailId = data.ProductDetailId,
                        CreateBy = data.UserId
                    };
                    await productRepo.AddAsync(price);
                    await unitOfWork.SaveChangesAsync();

                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };
            await _channel.BasicConsumeAsync(queue: "Price-Publish-Queue", autoAck: false, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);

        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null && _channel.IsOpen)
                await _channel.CloseAsync();

            if (_connection != null && _connection.IsOpen)
                await _connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
