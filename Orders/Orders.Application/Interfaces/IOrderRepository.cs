

using Orders.Domain.Entities;

namespace Orders.Application.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<Order> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
}
