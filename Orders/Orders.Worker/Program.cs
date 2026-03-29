using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Application.Services;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;
using Orders.Worker;
using Orders.Worker.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IMessageBus, NullMessageBus>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
