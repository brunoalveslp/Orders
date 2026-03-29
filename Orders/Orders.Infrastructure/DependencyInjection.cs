using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces;
using Orders.Application.Services;
using Orders.Infrastructure.Messaging;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddMessaging(configuration); // Registro síncrono de serviços

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<OrderService>();

        services.AddHostedService<MessagingInitializer>(); // Inicialização assíncrona em tempo de execução

        return services;
    }
}
