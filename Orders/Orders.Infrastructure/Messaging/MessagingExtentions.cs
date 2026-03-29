using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces;

namespace Orders.Infrastructure.Messaging;

public static class MessagingExtentions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageBus>(_ =>
            MessagingService.CreateAsync(configuration).GetAwaiter().GetResult());

        return services;
    }
}

