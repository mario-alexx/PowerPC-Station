using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

/// <summary>
/// Configures the properties and relationships of the <see cref="Product"/> entity using the Fluent API.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  /// <summary>
  /// Configures the schema needed for the <see cref="Product"/> entity.
  /// </summary>
  /// <param name="builder">The builder used to configure the entity.</param>
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
    builder.Property(p => p.Name).IsRequired().HasMaxLength(75);
    builder.Property(p => p.Description).IsRequired();
    builder.Property(p => p.Type).IsRequired().HasMaxLength(100);
    builder.Property(p => p.Brand).IsRequired().HasMaxLength(100);
    builder.Property(p => p.PictureUrl).IsRequired();
  }
}
