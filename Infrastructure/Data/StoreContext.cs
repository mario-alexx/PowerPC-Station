using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Represents the database context for the store, providing access to entities and configuration settings.
/// Initializes a new instance of the <see cref="StoreContext"/> class using the specified options.
/// </summary>
/// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
public class StoreContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<Product> Products{ get; set; }

  /// <summary>
  /// Configures the model that was discovered by convention from the entity types exposed in <see cref="DbSet{TEntity}"/> properties on the derived context.
  /// </summary>
  /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly( typeof(ProductConfiguration).Assembly );
  }
}
