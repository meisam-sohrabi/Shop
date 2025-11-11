using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.ApplicationContract.DTO.Permission;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Application.Services.ProductConsumer
{
    public class PermissionConsumerAppService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionConsumerAppService(IServiceScopeFactory scopeFactory)
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

                    await channel.QueueDeclareAsync(
                        queue: "Product-Permission-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Product-Permission-Queue",
                        exchange: "Account-Exchange",
                        routingKey: "PermissionCreated");

                    Console.WriteLine(" Connected to RabbitMQ and ready to consume.");

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var repoCmd = scope.ServiceProvider.GetRequiredService<IGenericCommandRepository<LocalPermissionEntity>>();

                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var data = JsonSerializer.Deserialize<PermissionEventDto>(message);
                            if (data == null)
                            {
                                Console.WriteLine("Invalid message received.");
                                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                                return;
                            }


                            var permission = new LocalPermissionEntity
                            {
                                Action = data.Action,
                                Id = data.Id,
                                Resource = data.Resource,
                                Description = data.Description,
                            };
                           await repoCmd.AddAsync(permission);
                            await unitOfWork.SaveChangesAsync();
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message: {ex.Message}");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                        }
                    };

                    await channel.BasicConsumeAsync("Product-Permission-Queue", false, consumer);

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
