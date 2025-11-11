using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PricingService.ApplicationContract.DTO.Price;
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


        public PriceConsumerAppService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //  ایجاد اتصال و کانال
                    using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(
                        queue: "Price-Publish-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Price-Publish-Queue",
                        exchange: "Product-Exchange",
                        routingKey: "AddPriceEvent");

                    Console.WriteLine(" Connected to RabbitMQ and ready to consume price messages.");

                    //  نگه‌داشتن ارتباط تا زمانی که کانکشن باز است
                    while (connection.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {
                        //  تعریف consumer
                        var consumer = new AsyncEventingBasicConsumer(channel);
                        consumer.ReceivedAsync += async (model, ea) =>
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                            var priceRepo = scope.ServiceProvider.GetRequiredService<IProductPriceCommandRepository>();

                            try
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                var data = JsonSerializer.Deserialize<PriceEventDto>(message);

                                if (data == null || data.ProductDetailId == 0)
                                {
                                    Console.WriteLine("Invalid message received.");
                                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                                    return;
                                }

                                var entity = new ProductPriceEntity
                                {
                                    ProductDetailId = data.ProductDetailId,
                                    Price = data.Price,
                                    CreateBy = data.UserId
                                };

                                await priceRepo.AddAsync(entity);
                                await unitOfWork.SaveChangesAsync();

                                await channel.BasicAckAsync(ea.DeliveryTag, false);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing price message: {ex.Message}");
                                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                            }
                        };

                        await channel.BasicConsumeAsync("Price-Publish-Queue", false, consumer);


                        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    }

                    Console.WriteLine(" Channel or connection closed. Attempting to reconnect...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" RabbitMQ connection failed: {ex.Message}");
                }

                // 💤 صبر برای reconnect
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
