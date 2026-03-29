using Microsoft.Extensions.Logging;
using Orders.Application.DTOs;
using Orders.Application.Events;
using Orders.Application.Interfaces;
using Orders.Application.Mappings;
using Orders.Domain.Common;
using Orders.Domain.Entities;
using System.Linq;

namespace Orders.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMessageBus _bus;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, IMessageBus bus, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _bus = bus;
        _logger = logger;
    }

    public async Task<Result<OrderResponse>> CreateAsync(CreateOrderRequest request)
    {
        var orderResult = Order.Create(request.NomeCliente, request.Descricao, request.Valor);

        if (!orderResult.IsSuccess)
            return Result<OrderResponse>.Failure(orderResult.Error);

        var order = orderResult.Value;

        try
        {
            await _orderRepository.CreateAsync(order);
            await _bus.PublishAsync("orders", new OrderCreatedEvent(order.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao persistir ou publicar pedido {OrderId}.", order.Id);
            return Result<OrderResponse>.Failure("Erro interno ao criar pedido.");
        }

        return Result<OrderResponse>.Success(order.ToResponse());
    }

    public async Task<Result<OrderResponse>> GetByIdAsync(Guid id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
                return Result<OrderResponse>.Failure("Order not found.");

            return Result<OrderResponse>.Success(order.ToResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pedido {OrderId}.", id);
            return Result<OrderResponse>.Failure("Erro interno ao buscar pedido.");
        }
    }

    public async Task<Result<List<OrderResponse>>> GetAllAsync()
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync();
            return Result<List<OrderResponse>>.Success(orders.Select(o => o.ToResponse()).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pedidos.");
            return Result<List<OrderResponse>>.Failure("Erro interno ao listar pedidos.");
        }
    }

    public async Task<Result<OrderResponse>> ProcessAsync(Guid id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order is null)
                return Result<OrderResponse>.Failure("Order not found.");

            var result = order.Process();

            if (!result.IsSuccess)
                return Result<OrderResponse>.Failure(result.Error);

            await Task.Delay(TimeSpan.FromSeconds(10));

            await _orderRepository.UpdateAsync(order);

            return Result<OrderResponse>.Success(order.ToResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar pedido {OrderId}.", id);
            return Result<OrderResponse>.Failure("Erro interno ao processar pedido.");
        }
    }
}
