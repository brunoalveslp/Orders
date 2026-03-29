using Orders.Application.Interfaces;

namespace Orders.Worker.Messaging;

public class NullMessageBus : IMessageBus
{
    public Task PublishAsync<T>(string queueName, T message) => Task.CompletedTask;
}
