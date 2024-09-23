using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

/// <summary>
/// Provides a method to seed the database with initial data for the store context.
/// </summary>
public class StoreContextSeed
{
  /// <summary>
  /// Asynchronously seeds the database with initial data if the database is empty.
  /// </summary>
  /// <param name="context">The store context used to access the database.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public static async Task SeedAsync(StoreContext context)
  {
    if(!context.Products.Any())
    {
      var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

      var products = JsonSerializer.Deserialize<List<Product>>(productsData);

      if(products == null) return;

      context.Products.AddRange(products);
      await context.SaveChangesAsync();
    }

     if(!context.DeliveryMethods.Any())
    {
      var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

      var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

      if(methods == null) return;

      context.DeliveryMethods.AddRange(methods);
      await context.SaveChangesAsync();
    }
  }
}
