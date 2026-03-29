using Orders.Application.DTOs;
using Orders.Domain.Entities;

namespace Orders.Application.Mappings;

public static class OrderMappings
{
    public static OrderResponse ToResponse(this Order order) =>
        new(order.Id, order.NomeCliente, order.Descricao, order.Valor, order.Status);
}
