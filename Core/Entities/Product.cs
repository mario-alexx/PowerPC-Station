namespace Core.Entities;

/// <summary>
/// Represents a product in the store, including details such as name, description, price, and stock quantity.
/// </summary>
public class Product : BaseEntity
{
  /// <summary>
  /// Gets or sets the name of the product.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// Gets or sets the description of the product.
  /// </summary>
  public required string Description { get; set; }

  /// <summary>
  /// Gets or sets the price of the product.
  /// </summary>
  public decimal Price { get; set; }

  /// <summary>
  /// Gets or sets the URL of the product's picture.
  /// </summary>
  public required string PictureUrl { get; set; }

  /// <summary>
  /// Gets or sets the type or category of the product.
  /// </summary>
  public required string Type { get; set; }

  /// <summary>
  /// Gets or sets the brand of the product.
  /// </summary>
  public required string Brand { get; set; }

  /// <summary>
  /// Gets or sets the quantity of the product in stock.
  /// </summary>
  public int QuantityInStock { get; set; }
}
