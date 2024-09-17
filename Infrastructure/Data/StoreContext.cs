using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Represents the database context for the store application, including identity functionality for users.
/// </summary>
/// <param name="options">Options to configure the database context <see cref="IdentityDbContext{AppUser}"/>.</param>
public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
  public DbSet<Product> Products{ get; set; }
  public DbSet<Address> Addresses{ get; set; }

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
