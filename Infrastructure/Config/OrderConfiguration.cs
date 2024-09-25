using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

/// <summary>
/// Configuration for the Order entity.
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{ 
  /// <summary>
  /// Configures the properties and relationships of the Order entity.
  /// </summary>
  /// <param name="builder">The builder used to configure the entity.</param>
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());
    builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
    builder.Property(x => x.Status).HasConversion(
      o => o.ToString(),
      o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
    );
    builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
    builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
    builder.Property(x => x.OrderDate).HasConversion(
      d => d.ToUniversalTime(),
      d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
    );
  }
}
