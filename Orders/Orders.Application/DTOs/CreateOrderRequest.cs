namespace Order.Application.DTOs;

public record CreateOrderRequest(
    string NomeCliente,
    string Descricao,
    decimal Valor
);