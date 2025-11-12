using AccountService.InfrastructureContract.Interfaces;
using AccountService.InfrastructureContract.Interfaces.Command.OutBox;
using AccountService.InfrastructureContract.Interfaces.Query.OutBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text;

namespace AccountService.Application.Services.OutBoxProcessor
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
                    using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connection.CreateChannelAsync();
                    await channel.ExchangeDeclareAsync(
                        exchange: "Account-Exchange",
                        type: ExchangeType.Topic,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    while (connection.IsOpen && !channel.IsClosed && !stoppingToken.IsCancellationRequested)
                    {

                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var priceCmd = scope.ServiceProvider.GetRequiredService<IOutBoxCommandRepository>();
                        var priceQry = scope.ServiceProvider.GetRequiredService<IOutBoxQueryRepository>();

                        var messages = await priceQry.GetQueryable()
                                .Where(c => !c.Sent)
                                .ToListAsync(stoppingToken);
                        foreach (var msg in messages)
                        {
                            try
                            {
                              await  PublishAsync(msg.Event, msg.Content, channel, properties);
                                msg.Sent = true;
                                msg.SentAt = DateTime.Now;
                                priceCmd.Edit(msg);
                                Console.WriteLine($"***************event {msg.Event}");
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
                        exchange: "Account-Exchange",
                        routingKey: eventType,
                        mandatory: true,
                        basicProperties: property,
                        body: Encoding.UTF8.GetBytes(content));
        }
    }
}
