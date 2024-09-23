using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

/// <summary>
/// Configures the properties for the <see cref="DeliveryMethod"/> entity.
/// </summary>
public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{ 
  /// <summary>
  /// Configures the entity type for DeliveryMethod, setting specific rules for properties.
  /// </summary>
  /// <param name="builder">The builder to configure the DeliveryMethod entity.</param>
  public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
  {
    builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
  }
}
