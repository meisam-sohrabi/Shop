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
    public class OutBoxPriceProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutBoxPriceProcessor(IServiceScopeFactory scopeFactory)
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
                        exchange: "Price-Publish-Exchange",
                        type: ExchangeType.Direct,
                        durable: true, autoDelete: false,
                        arguments: null);

                    await channel.QueueDeclareAsync(
                        queue: "Price-Publish-Queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(
                        queue: "Price-Publish-Queue",
                        exchange: "Price-Publish-Exchange",
                        routingKey: "Price-Publish-RoutingKey",
                        arguments: null);

                    //  حلقه داخلی: تا زمانی که connection سالمه
                    while (connectoin.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var priceCmd = scope.ServiceProvider.GetRequiredService<IOutBoxCommandRepository>();
                        var priceQry = scope.ServiceProvider.GetRequiredService<IOutBoxQueryRepository>();

                        var messages = await priceQry.GetQueryable()
                            .Where(c => !c.Sent && c.Event == "AddPriceEvent")
                            .ToListAsync(stoppingToken);

                        foreach (var msg in messages)
                        {
                            try
                            {
                                await channel.BasicPublishAsync(
                                    exchange: "Price-Publish-Exchange",
                                    routingKey: "Price-Publish-RoutingKey",
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
