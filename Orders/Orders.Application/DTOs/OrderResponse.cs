using Orders.Domain.Enums;

namespace Orders.Application.DTOs;

public record OrderResponse(
    Guid Id,
    string NomeCliente,
    string Descricao,
    decimal Valor,
    OrderStatus Status
);
