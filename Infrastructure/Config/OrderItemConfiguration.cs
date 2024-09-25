using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

/// <summary>
/// Configuration for the OrderItem entity.
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
  /// <summary>
  /// Configures the properties and relationships of the OrderItem entity.
  /// </summary>
  /// <param name="builder">The builder used to configure the entity.</param>
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.OwnsOne(x => x.ItemOrdered, o => o.WithOwner());
    builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
  }
}
