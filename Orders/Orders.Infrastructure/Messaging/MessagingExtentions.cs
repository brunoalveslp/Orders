using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces;

namespace Orders.Infrastructure.Messaging;

public static class MessagingExtentions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageBus>(_ =>
        {
            try
            {
                return MessagingService.CreateAsync(configuration).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Falha ao conectar ao RabbitMQ durante o startup. Verifique as configurações e se o serviço está disponível.", ex);
            }
        });

        return services;
    }
}

