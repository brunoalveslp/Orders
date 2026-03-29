
using Orders.Application.DTOs;
using Orders.Application.Interfaces;
using Orders.Application.Mappings;
using Orders.Domain.Common;
using Orders.Domain.Entities;
using System.Linq;

namespace Orders.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;    

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderResponse>> CreateAsync(CreateOrderRequest request)
    {
        var orderResult = Order.Create(request.NomeCliente, request.Descricao, request.Valor);

        if (!orderResult.IsSuccess)
            return Result<OrderResponse>.Failure(orderResult.Error);

        var order = orderResult.Value;

        await _orderRepository.CreateAsync(order);

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
}
