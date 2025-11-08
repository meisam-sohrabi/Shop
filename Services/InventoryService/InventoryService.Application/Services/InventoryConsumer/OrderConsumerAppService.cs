using InventoryService.ApplicationContract.DTO.Order;
using InventoryService.ApplicationContract.DTO.ProductInventory;
using InventoryService.Domain.Entities;
using InventoryService.InfrastructureContract.Interfaces;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryService.Application.Services.InventoryConsumer
{
    public class OrderConsumerAppService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderConsumerAppService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // اتصال و کانال
                    using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connection.CreateChannelAsync();

                    await channel.ExchangeDeclareAsync(
                        exchange: "Order-Publish-Exchange",
                        type: ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueDeclareAsync(
                        queue: "Order-Publish-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Order-Publish-Queue",
                        exchange: "Order-Publish-Exchange",
                        routingKey: "Order-Publish-RoutingKey");

                    Console.WriteLine(" Connected to RabbitMQ and ready to consume.");

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var repo = scope.ServiceProvider.GetRequiredService<IProductInventoryCommandRepository>();

                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var data = JsonSerializer.Deserialize<OrderEventDto>(message);

                            if (data == null || data.ProductDetailId == 0)
                            {
                                Console.WriteLine("Invalid message received.");
                                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                                return;
                            }

                            var entity = new ProductInventoryEntity
                            {
                                ProductDetailId = data.ProductDetailId,
                                QuantityChange = data.QuantityChange,
                                CreateBy = data.UserId
                            };

                            await repo.AddAsync(entity);
                            await unitOfWork.SaveChangesAsync();
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message: {ex.Message}");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                        }
                    };

                    await channel.BasicConsumeAsync("Order-Publish-Queue", false, consumer);

                    // دلیل وجود این حلقه تکراری برای حفظ ارتباط هستش و جلوگیری از ساخت کانسیومر جدید
                    // مدیریت ریسورس
                    while (channel.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }

                    Console.WriteLine("Channel or connection closed. Reconnecting...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RabbitMQ connection failed: {ex.Message}");
                }

                // وقتی که ارتباط قطع شد 5 ثانیه صبر کن و مجدد تلاش کن
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
