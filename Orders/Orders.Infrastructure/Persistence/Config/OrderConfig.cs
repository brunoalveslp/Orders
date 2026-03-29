using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence.Config;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.NomeCliente)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Descricao)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.Valor)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
