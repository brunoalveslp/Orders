using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
       return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
