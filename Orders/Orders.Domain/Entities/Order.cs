using Orders.Domain.Common;
using Orders.Domain.Enums;

namespace Orders.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public string NomeCliente { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public OrderStatus Status { get; private set; }

    private Order(string nomeCliente, string descricao, decimal valor)
    {
        NomeCliente = nomeCliente;
        Descricao = descricao;
        Valor = valor;
    }

    public static Result<Order> Create (string nomeCliente, string descricao, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(nomeCliente))
            return Result<Order>.Failure("O nome do cliente é obrigatório.");

        if (valor <= 0)
            return Result<Order>.Failure("O valor do pedido deve ser maior que zero.");

        var order = new Order(nomeCliente, descricao, valor)
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Pendente
        };

        return Result<Order>.Success(order);
    }

    public Result Process()
    {
        if (Status != OrderStatus.Pendente)
            return Result.Failure("O pedido já foi processado.");

        Status = OrderStatus.Processado;
        return Result.Success();
    }
}