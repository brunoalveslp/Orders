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

    public OrderService(IOrderRepository orderRepository, IMessageBus bus)
    {
        _orderRepository = orderRepository;
        _bus = bus;
    }

    public async Task<Result<OrderResponse>> CreateAsync(CreateOrderRequest request)
    {
        var orderResult = Order.Create(request.NomeCliente, request.Descricao, request.Valor);

        if (!orderResult.IsSuccess)
            return Result<OrderResponse>.Failure(orderResult.Error);

        var order = orderResult.Value;

        await _orderRepository.CreateAsync(order);

        await _bus.PublishAsync("orders", new OrderCreatedEvent(order.Id));

        return Result<OrderResponse>.Success(order.ToResponse());
    }

    public async Task<Result<OrderResponse>> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return Result<OrderResponse>.Failure("Order not found.");

        return Result<OrderResponse>.Success(order.ToResponse());
    }

    public async Task<Result<List<OrderResponse>>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Result<List<OrderResponse>>.Success(orders.Select(o => o.ToResponse()).ToList());
    }

    public async Task<Result<OrderResponse>> ProcessAsync(Guid id)
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
}
