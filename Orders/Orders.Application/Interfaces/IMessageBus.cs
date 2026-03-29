namespace Orders.Application.Interfaces;

public interface IMessageBus
{
    Task PublishAsync(string queueName, object message);
}
