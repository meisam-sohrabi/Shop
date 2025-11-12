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
    public class OutBoxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutBoxProcessor(IServiceScopeFactory scopeFactory)
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
                        exchange: "Order-Exchange",
                        type: ExchangeType.Direct,
                        durable: true, autoDelete: false,
                        arguments: null);

                    //  حلقه داخلی: تا زمانی که connection سالمه
                    while (connectoin.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var orderCmd = scope.ServiceProvider.GetRequiredService<IOutBoxCommandRepository>();
                        var orderQry = scope.ServiceProvider.GetRequiredService<IOutBoxQueryRepository>();

                        var messages = await orderQry.GetQueryable()
                            .Where(c => !c.Sent)
                            .ToListAsync(stoppingToken);

                        foreach (var msg in messages)
                        {
                            try
                            {
                                await PublishAsync(msg.Event, msg.Content, channel, properties);

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

        private async Task PublishAsync(string eventType, string content, IChannel channel, BasicProperties property)
        {
            await channel.BasicPublishAsync(
                        exchange: "Order-Exchange",
                        routingKey: eventType,
                        mandatory: true,
                        basicProperties: property,
                        body: Encoding.UTF8.GetBytes(content));
        }
    }
}
