using Orders.Application.DTOs;
using Orders.Application.Interfaces;
using Orders.Domain.Common;
using Orders.Domain.Entities;
using System.Linq;

namespace Orders.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;    

    public OrderService(IOrderRepository orderRepository, IMessageBus messageBus)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Order>> CreateAsync(CreateOrderRequest request)
    {
        var orderResult = Order.Create(request.NomeCliente, request.Descricao, request.Valor);

        if (!orderResult.IsSuccess)
            return Result<Order>.Failure(orderResult.Error);

        var order = orderResult.Value;

        await _orderRepository.CreateAsync(order);

        return Result<Order>.Success(order);
    }

    public async Task<Result<Order>> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return Result<Order>.Failure("Order not found.");
        return Result<Order>.Success(order);
    }

    public async Task<Result<List<Order>>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Result<List<Order>>.Success(orders.ToList());
    }
}
