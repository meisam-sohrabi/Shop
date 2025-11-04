using InventoryService.ApplicationContract.DTO.ProductInventory;
using InventoryService.Domain.Entities;
using InventoryService.InfrastructureContract.Interfaces;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using Microsoft.Extensions.Hosting;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryService.Application.Services.RabbitInventory
{
    public class RabbitInventoryConsumerAppService : BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductInventoryCommandRepository _productInventoryCommandRepository;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitInventoryConsumerAppService(IUnitOfWork unitOfWork, IProductInventoryCommandRepository productInventoryCommandRepository)
        {
            _unitOfWork = unitOfWork;
            _productInventoryCommandRepository = productInventoryCommandRepository;
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
                await _channel.ExchangeDeclareAsync(exchange: "PlaceOrder-Exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                await _channel.QueueDeclareAsync(queue: "PlaceOrder-Queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                await _channel.QueueBindAsync(queue: "PlaceOrder-Queue", exchange: "PlaceOrder-Exchange", routingKey: "PlaceOrder-RoutingKey", arguments: null);
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
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var data = JsonSerializer.Deserialize<ProductInventoryRequestDto>(message);
                    if (data == null || data.ProductId == 0)
                    {
                        Console.WriteLine("Received invalid or incomplete message.");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                        return;
                    }
                    var inventory = new ProductInventoryEntity
                    {
                        QuantityChange = data.QuantityChange,
                        ProductId = data.ProductId
                    };
                    await _productInventoryCommandRepository.AddAsync(inventory);
                    await _unitOfWork.SaveChangesAsync();

                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };
            await _channel.BasicConsumeAsync(queue: "PlaceOrder-Queue", autoAck: false, consumer: consumer);
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
