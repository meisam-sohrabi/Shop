using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.InfrastructureContract.Interfaces;
using OrderService.InfrastructureContract.Interfaces.Command.OutBox;
using OrderService.InfrastructureContract.Interfaces.Query.OutBox;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.Application.Services.OutBoxProcessor
{
    public class OutBoxProductProccessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutBoxProductProccessor(IServiceScopeFactory scopeFactory)
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
                    using var connectoin = await factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connectoin.CreateChannelAsync();
                    await channel.ExchangeDeclareAsync(
                        exchange: "Product-Publish-Exchange",
                        type: ExchangeType.Direct,
                        durable: true, autoDelete: false,
                        arguments: null);

                    await channel.QueueDeclareAsync(
                        queue: "Product-Publish-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Product-Publish-Queue",
                        exchange: "Product-Publish-Exchange",
                        routingKey: "Product-Publish-RoutingKey",
                        arguments: null);

                    //  حلقه داخلی: تا زمانی که connection سالمه
                    while (connectoin.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var orderCmd = scope.ServiceProvider.GetRequiredService<IOutBoxCommandRepository>();
                        var orderQry = scope.ServiceProvider.GetRequiredService<IOutBoxQueryRepository>();

                        var messages = await orderQry.GetQueryable()
                            .Where(c => !c.Sent && c.Event == "ReduceProductEvent")
                            .ToListAsync(stoppingToken);

                        foreach (var msg in messages)
                        {
                            try
                            {
                                await channel.BasicPublishAsync(
                                    exchange: "Product-Publish-Exchange",
                                    routingKey: "Product-Publish-RoutingKey",
                                    mandatory: true,
                                    basicProperties: properties,
                                    body: Encoding.UTF8.GetBytes(msg.Content)
                                );

                                msg.Sent = true;
                                msg.SentAt = DateTime.Now;
                                orderCmd.Edit(msg);
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
