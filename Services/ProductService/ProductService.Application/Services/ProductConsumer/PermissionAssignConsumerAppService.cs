using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.ApplicationContract.DTO.UserPermission;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Application.Services.ProductConsumer
{
    public class PermissionAssignConsumerAppService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAssignConsumerAppService(IServiceScopeFactory scopeFactory)
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
                        queue: "PermissionAssign-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "PermissionAssign-Queue",
                        exchange: "Account-Exchange",
                        routingKey: "UserPermission.*");

                    Console.WriteLine(" Connected to RabbitMQ and ready to consume.");

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var repoCmd = scope.ServiceProvider.GetRequiredService<IGenericCommandRepository<LocalUserPermissionEntity>>();

                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var data = JsonSerializer.Deserialize<UserPermissionDto>(message);
                            if (data == null)
                            {
                                Console.WriteLine("Invalid message received.");
                                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                                return;
                            }


                            var userPermission = new LocalUserPermissionEntity
                            {
                                UserId = data.UserId,
                                PermissionId = data.PermissionId
                            };
                            await repoCmd.AddAsync(userPermission);
                            await unitOfWork.SaveChangesAsync();
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message: {ex.Message}");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                        }
                    };

                    await channel.BasicConsumeAsync("PermissionAssign-Queue", false, consumer);

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
