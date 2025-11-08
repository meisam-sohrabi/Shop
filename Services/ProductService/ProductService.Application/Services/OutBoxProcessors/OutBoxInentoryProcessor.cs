using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.OutBox;
using ProductService.InfrastructureContract.Interfaces.Query.OutBox;
using RabbitMQ.Client;
using System.Text;

namespace ProductService.Application.Services.OutBoxProcessors
{
    public class OutBoxInentoryProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutBoxInentoryProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var properties = new BasicProperties { Persistent = true };
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connection.CreateChannelAsync();
                    await channel.ExchangeDeclareAsync(
                        exchange: "Inventory-Exchange",
                        type: ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueDeclareAsync(
                        queue: "Inventory-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Inventory-Queue",
                        exchange: "Inventory-Exchange",
                        routingKey: "Inventory-RoutingKey",
                        arguments: null);

                    while (connection.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {

                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var priceCmd = scope.ServiceProvider.GetRequiredService<IOutBoxCommandRepository>();
                        var priceQry = scope.ServiceProvider.GetRequiredService<IOutBoxQueryRepository>();

                        var messages = await priceQry.GetQueryable()
                                .Where(c => !c.Sent && c.Event == "AddInventoryEvent")
                                .ToListAsync(stoppingToken);
                        foreach (var msg in messages)
                        {
                            try
                            {
                                await channel.BasicPublishAsync(
                                    exchange: "Inventory-Exchange",
                                    routingKey: "Inventory-RoutingKey",
                                    mandatory: true,
                                    basicProperties: properties,
                                    body: Encoding.UTF8.GetBytes(msg.Content)
                                );

                                msg.Sent = true;
                                msg.SentAt = DateTime.Now;
                                priceCmd.Edit(msg);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Publish error for message {msg.Id}: {ex.Message}");
                            }
                        }

                        await uow.SaveChangesAsync();
                        await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured, Message : {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            }
        }
    }
}
