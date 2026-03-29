using Order.Domain.Entities;

namespace Order.Application.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order);
}
