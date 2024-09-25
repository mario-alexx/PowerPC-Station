namespace Core.Entities.OrderAggregate;

/// <summary>
/// Represents a product item that was ordered.
/// </summary>
public class ProductItemOrdered
{
  /// <summary>
  /// The ID of the product.
  /// </summary>
  public int ProductId { get; set; }

  /// <summary>
  /// The name of the product.
  /// </summary>
  public required string ProductName { get; set; }

  /// <summary>
  /// The URL of the product's picture.
  /// </summary>
  public required string PictureUrl { get; set; }
}
