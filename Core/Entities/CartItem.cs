namespace Core.Entities;

/// <summary>
/// Represents an item in the shopping cart, including product details and quantity.
/// </summary>
public class CartItem
{
  /// <summary>
  /// Gets or sets the unique identifier for the product.
  /// </summary>
  public int ProductId { get; set; }

  /// <summary>
  /// Gets or sets the name of the product.
  /// This field is required.
  /// </summary>
  public required string ProductName { get; set; }

  /// <summary>
  /// Gets or sets the price of a single unit of the product.
  /// </summary>
  public decimal Price { get; set; }

  /// <summary>
  /// Gets or sets the quantity of the product in the cart.
  /// </summary>
  public int Quantity { get; set; }

  /// <summary>
  /// Gets or sets the URL of the product's image.
  /// This field is required.
  /// </summary>
  public required string PictureUrl { get; set; }

  /// <summary>
  /// Gets or sets the brand of the product.
  /// This field is required.
  /// </summary>
  public required string Brand { get; set; }

  /// <summary>
  /// Gets or sets the type or category of the product.
  /// This field is required.
  /// </summary>
  public required string Type { get; set; }
}
