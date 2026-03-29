using Microsoft.Extensions.Configuration;
using Orders.Application.Interfaces;
using RabbitMQ.Client;

namespace Orders.Infrastructure.Messaging;

public class MessagingService : IMessageBus
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public MessagingService(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public static async Task<MessagingService> CreateAsync(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "rabbitmq",
            UserName = configuration["RabbitMQ:UserName"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        return new MessagingService(connection, channel);
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        await _channel.QueueDeclareAsync(queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

        var json = System.Text.Json.JsonSerializer.Serialize(message);
        var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(exchange: "",
                                         routingKey: queueName,
                                         mandatory: true,
                                         basicProperties: props,
                                         body: messageBodyBytes);
    }
}

