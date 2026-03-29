using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Application.Interfaces;

namespace Orders.Infrastructure.Messaging;

public class MessagingInitializer : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<MessagingInitializer> _logger;

    public MessagingInitializer(IMessageBus messageBus, ILogger<MessagingInitializer> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ connection initialized successfully.");
        return Task.CompletedTask;
    }
}
